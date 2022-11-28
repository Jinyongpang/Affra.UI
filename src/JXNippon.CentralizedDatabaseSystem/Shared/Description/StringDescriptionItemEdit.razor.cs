using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class StringDescriptionItemEdit<TItem>
        where TItem : class
    {
		[Parameter] public bool IsEditable { get; set; }
		[Parameter] public string StringVal { get; set; }
        [Parameter] public TItem Item { get; set; }
        [Parameter] public long ItemId { get; set; }
        [Parameter] public bool IsRequired { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Parameter] public EventCallback<string?> StringValChanged { get; set; }
        [Parameter] public EventCallback<string?> OnStringValChanged { get; set; }

        private IGenericService<TItem> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private bool isEditing;

        private async Task StartEdit()
        {
            if(IsEditable && await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.CombinedDailyReport), HasReadPermissoin = true, HasWritePermission = true }))
            {
				isEditing = true;
				this.StateHasChanged();
			}   
        }

        private async Task StopEdit()
        {
            try
            {
                await StringValChanged.InvokeAsync(this.StringVal);
                await OnStringValChanged.InvokeAsync(this.StringVal);

                using var scope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(scope);

                if (this.Item.AsIEntity().Id > 0)
                {
                    await service.UpdateAsync(this.Item, this.Item.AsIEntity().Id);
                    AffraNotificationService.NotifyItemUpdated();
                }
                else
                {
                    await service.InsertAsync(this.Item);
                    AffraNotificationService.NotifyItemCreated();
                }


                isEditing = false;
                this.StateHasChanged();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private string GetStyle()
        {
            return IsRequired && this.StringVal is null
                ? "background-color: #FFFF99;"
                : null;
        }

        private void MouseLeave()
        {
            isEditing = false;
            this.StateHasChanged();
        }
    }
}
