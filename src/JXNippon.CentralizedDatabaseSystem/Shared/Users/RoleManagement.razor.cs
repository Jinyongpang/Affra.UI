using Affra.Core.Domain.Services;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class RoleManagement
    {
        private Menu menu;
        private string search;
        private ICollection<Role> roles;
        private RolePermissionManagement rolePermissionManagement;
        private Role item;
        private bool isUserHavePermission = true;

        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            isUserHavePermission = await userService.CheckHasPermissionAsync(null, new Permission { Name = "Administration", HasReadPermissoin = true, HasWritePermission = true });
            roles = await userService.GetRolesAsync();
            roles = roles
                .Where(x => x.Name != "Administrator")
                .ToList();

            var roleName = roles.FirstOrDefault()?.Name;

            if (roleName is not null)
            {
                menu.SelectedKeys = new[] { roleName, };
                await this.ReloadAsync(roleName);
            }
            
            await base.OnInitializedAsync();
        }

        private Task ReloadAsync(string roleName = null)
        {
            item = roles
                .Where(x => x.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return (this.rolePermissionManagement?.ReloadAsync() ?? Task.CompletedTask);
        }

        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            return this.ReloadAsync(menuItem.Key);
        }
        private async Task DeleteAsync(Role role)
        {
            var response = await DialogService.OpenAsync<GenericConfirmationDialog>("Delete Role?",
            new Dictionary<string, object>() { },
            new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<Role, IUserUnitOfWork>>();
                    await service.DeleteAsync(role);
                    this.roles.Remove(role);
                    this.StateHasChanged();
                    AffraNotificationService.NotifyItemDeleted();
                }
                catch (Exception ex)
                { 
                    AffraNotificationService.NotifyException(ex);
                }
                
            }
        }

        private async Task AddRoleAsync()
        {
            var permissions = Enum.GetValues(typeof(FeaturePermission))
                .Cast<FeaturePermission>()
                .OrderBy(x => (int)x)
                .Select(x => new Permission() { Name = x.ToString(), HasReadPermissoin = true, HasWritePermission = false, })
                .ToList();
            Role item = new Role()
            {
                Permissions = new (),
            };
            foreach (var permission in permissions)
            {
                item.Permissions.Add(new Permission()
                {
                    Name = permission.Name,
                });
            }
            
            dynamic? response = await DialogService.OpenAsync<RoleDialog>("Add Role",
                           new Dictionary<string, object>() { { "Item", item } },
                           Constant.DialogOptions);

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<Role, IUserUnitOfWork>>();
                    await service.InsertAsync(item);
                    this.roles.Add(item);
                    StateHasChanged();
                    AffraNotificationService.NotifyItemCreated();
                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
                finally
                {
                }
            }
        }
    }
}
