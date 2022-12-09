using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Radzen;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class UserDataList
    {
        private Virtualize<User> virtualize;
        private int count;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
        private ListGridType grid = new()
        {
            Gutter = 16,
            Xs = 1,
            Sm = 1,
            Md = 1,
            Lg = 2,
            Xl = 3,
            Xxl = 3,
        };

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IUserService UserService { get; set; }

        public CommonFilter CommonFilter { get; set; }

        protected override Task OnInitializedAsync()
        {
            this.CommonFilter = new CommonFilter(navigationManager);
            return Task.CompletedTask;
        }

        public async Task ReloadAsync()
        {
            await this.virtualize.RefreshDataAsync();
            this.StateHasChanged();
        }

        private async ValueTask<ItemsProviderResult<User>> LoadDataAsync(ItemsProviderRequest request)
        {
            isLoading = true;
            StateHasChanged();

            try
            {
                isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.Administration), HasReadPermissoin = true, HasWritePermission = true });
                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<User>? userService = this.GetGenericService(serviceScope);
                var query = userService.Get();
                if (!string.IsNullOrEmpty(CommonFilter.Status))
                {
                    query = query.Where(x => x.Role.ToUpper() == CommonFilter.Status.ToUpper());
                }
                if (!string.IsNullOrEmpty(CommonFilter.Search))
                {
                    query = query.Where(x => x.Username.ToUpper().Contains(CommonFilter.Search.ToUpper())
                     || x.Email.ToUpper().Contains(CommonFilter.Search.ToUpper())
                     || x.Name.ToUpper().Contains(CommonFilter.Search.ToUpper()));
                }
                Microsoft.OData.Client.QueryOperationResponse<User>? usersResponse = await query
                    .OrderByDescending(user => user.Id)
                    .Skip(request.StartIndex)
                    .Take(request.Count)
                    .ToQueryOperationResponseAsync<User>();

                count = (int)usersResponse.Count;
                var usersList = usersResponse.ToList();

                isLoading = false;
                return new ItemsProviderResult<User>(usersList, count);
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
            finally
            {
                initLoading = false;
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task ShowActivityDialogAsync(User user)
        {
            await this.DialogService.OpenAsync<UserActivityTable>("View Activities",
            new Dictionary<string, object>() { ["User"] = user },
            new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true, });

        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<User> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<User, IUserUnitOfWork>>();
        }

        private string GetAvatarIcon(UserPersonalization userPersonalization)
        {
            return userPersonalization?.AvatarId > 0
                     ? $"avatar\\{userPersonalization?.AvatarId}.png"
                     : string.Empty;
        }
        private async Task ShowDialogAsync(User data, int menuAction, string title)
        {
            dynamic? response;
            if (menuAction == 2)
            {
                response = await DialogService.OpenAsync<GenericConfirmationDialog>(title,
                           new Dictionary<string, object>() { },
                           new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

                if (response == true)
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService(serviceScope);
                    await service.DeleteAsync(data);

                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<UserDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction }, { "IsUserEdit", false }, },
                           Constant.DialogOptions);

                if (response == true)
                {
                    try
                    {
                        using var serviceScope = ServiceProvider.CreateScope();
                        var service = this.GetGenericService(serviceScope);

                        if (data.Id != Guid.Empty)
                        {
                            isLoading = true;
                            await service.UpdateAsync(data, data.Id);
                            AffraNotificationService.NotifyItemUpdated();
                        }
                        else
                        {
                            isLoading = true;
                            await service.InsertAsync(data);
                            AffraNotificationService.NotifyItemCreated();
                        }

                    }
                    catch (Exception ex)
                    {
                        AffraNotificationService.NotifyException(ex);
                    }
                    finally
                    {
                        isLoading = false;
                    }
                }
            }
        }

    }
}
