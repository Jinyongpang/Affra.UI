using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using Microsoft.AspNetCore.Components;

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

        protected override async Task OnInitializedAsync()
        {
            roles = await UserService.GetRolesAsync();
            await base.OnInitializedAsync();
        }

        private Task ReloadAsync(string status = null)
        {
            userDataList.CommonFilter.Status = this.GetStatusFilter(status);
            userDataList.CommonFilter.Search = search;
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
