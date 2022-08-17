using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class UserDataList
    {
        private const int loadSize = 9;
        private AntList<User> _dataList;
        private List<User> users;
        private int count;
        private int currentCount;
        private bool isLoading = false;
        private bool initLoading = true;
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
            initLoading = false;
            return this.LoadDataAsync();
        }

        public Task ReloadAsync()
        {
            return this.LoadDataAsync();
        }

        public Task OnLoadMoreAsync()
        {
            return this.LoadDataAsync(true);
        }

        private async Task LoadDataAsync(bool isLoadMore = false)
        {
            isLoading = true;
            StateHasChanged();
            if (!isLoadMore)
            {
                currentCount = 0;
            }

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<User>? userService = this.GetGenericService(serviceScope);
            var query = userService.Get();
            if (!string.IsNullOrEmpty(CommonFilter.Search))
            {
                query = query.Where(x => x.Username.ToUpper().Contains(CommonFilter.Search.ToUpper())
                 || x.Email.ToUpper().Contains(CommonFilter.Search.ToUpper())
                 || x.Name.ToUpper().Contains(CommonFilter.Search.ToUpper()));
            }
            Microsoft.OData.Client.QueryOperationResponse<User>? usersResponse = await query
                .OrderByDescending(user => user.Id)
                .Skip(currentCount)
                .Take(loadSize)
                .ToQueryOperationResponseAsync<User>();

            count = (int)usersResponse.Count;
            currentCount += loadSize;
            var usersList = usersResponse.ToList();

            if (isLoadMore)
            {
                users.AddRange(usersList);
            }
            else
            {
                users = usersList;
            }

            isLoading = false;

            if (usersList.DistinctBy(x => x.Id).Count() != usersList.Count)
            {
                AffraNotificationService.NotifyWarning("Data have changed. Kindly reload.");
            }

            StateHasChanged();
        }

        private async Task ShowActivityDialogAsync(User user)
        {
            await this.DialogService.OpenAsync<UserActivityTable>("View Activities",
            new Dictionary<string, object>() { ["User"] = user },
            new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<User> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<User, IUserUnitOfWork>>();
        }
        public async Task ShowDialogAsync(User data, int menuAction, string title)
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
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction }, },
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
