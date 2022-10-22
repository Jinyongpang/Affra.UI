using System.Collections.ObjectModel;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class RolePermissionManagement
    {
        [Parameter] public Role Item { get; set; }
        [Inject] private IUserService UserService { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }

        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        private ICollection<Permission> _permissions;
        private Commons.CollectionView<Permission> collectionView;

        protected override async Task OnInitializedAsync()
        {       
            this._permissions = Enum.GetValues(typeof(FeaturePermission))
                .Cast<FeaturePermission>()
                .OrderBy(x => (int)x)
                .Select(x => new Permission() { Name = x.ToString(), HasReadPermissoin = true, HasWritePermission = false, })
                .ToList();
            
            await this.ReloadAsync();
        }

        protected async Task SubmitAsync(Role arg)
        {
            try
            { 
                using var serviceScope = ServiceProvider.CreateScope();
                var service = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<Role, IUserUnitOfWork>>();
                await service.UpdateAsync(arg, arg.Id);
                this.AffraNotificationService.NotifyItemUpdated();
            }
            catch (Exception ex)
            { 
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        public async Task ReloadAsync()
        {
            if (Item is null)
            {
                return;
            }
            using var scope = ServiceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            Item = await userService.GetRoleAsync(Item.Name);
            if (Item.Permissions is null)
            {
                Item.Permissions = new Collection<Permission>();
            }
            foreach (var permission in this._permissions)
            {
                if (Item.Permissions?.Any(x => x.Name == permission.Name) == false)
                {
                    Item.Permissions.Add(new Permission() { 
                        Name = permission.Name,
                    });
                }
            }
            await (collectionView?.ReloadAsync() ?? Task.CompletedTask);
            this.StateHasChanged();
        }


    }
}
