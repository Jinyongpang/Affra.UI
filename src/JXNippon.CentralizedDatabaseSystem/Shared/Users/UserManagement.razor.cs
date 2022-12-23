using Affra.Core.Domain.Services;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class UserManagement
    {
        private const string All = "All";
        private UserDataList userDataList;
        private Menu menu;
        private string search;
        private ICollection<Role> roles;

        [Inject] private IUserService UserService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            roles = await UserService.GetRolesAsync();
            await base.OnInitializedAsync();
        }

        private async Task OnClickAsync(RadzenSplitButtonItem item)
        {
            if (item?.Value == "New")
            {
                await AddNewUserAsync();
            }
            else if (item?.Value == "Import")
            {
                await DialogService.OpenAsync<UserImportDialog>("Import Users",
                    new Dictionary<string, object>() { },
                    Constant.DialogOptions);

                await ReloadAsync();
            }
            else
            {
                await ReloadAsync();
            }
        }

        private async Task AddNewUserAsync()
        {
            User data = new();
            var response = await DialogService.OpenAsync<UserDialog>("New User",
                              new Dictionary<string, object>() { { "Item", data }, { "MenuAction", 1 }, { "IsUserEdit", false }, },
                              Constant.DialogOptions);

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = GetGenericService(serviceScope);

                    await service.InsertAsync(data);
                    AffraNotificationService.NotifyItemCreated();

                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
                finally
                {
                    await userDataList.ReloadAsync();
                }
            }

        }
        private IGenericService<User> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<User, IUserUnitOfWork>>();
        }

        private Task ReloadAsync(string status = null, string searchInput = null)
        {
            userDataList.CommonFilter.Status = this.GetStatusFilter(status);
            userDataList.CommonFilter.Search = searchInput ?? search;
            return userDataList.ReloadAsync();
        }


        private string GetStatusFilter(string status = null)
        {
            status ??= menu.SelectedKeys.FirstOrDefault();
            if (status != All)
            {
                return status;
            }

            return null;
        }

        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            return this.ReloadAsync(menuItem.Key);
        }
    }
}
