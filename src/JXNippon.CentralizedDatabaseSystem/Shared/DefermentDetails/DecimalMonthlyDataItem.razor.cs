using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class DecimalMonthlyDataItem<TItem>
        where TItem : class
    {
        [Parameter] public decimal? Value { get; set; }
        [Parameter] public TItem Item { get; set; }
        [Parameter] public long ItemId { get; set; }
        [Parameter] public bool IsEditable { get; set; } = false;
        [Parameter] public bool IsRequired { get; set; } = false;
        [Parameter] public string Year { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Parameter] public EventCallback<decimal?> ValueChanged { get; set; }
        [Parameter] public EventCallback<string> OnDataUpdate { get; set; }

        private IGenericService<TItem> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private bool isEditing;

        private async Task StartEdit()
        {
            if (IsEditable && await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.Deferment), HasReadPermissoin = true, HasWritePermission = true }))
            {
                isEditing = true;
                this.StateHasChanged();
            }
        }

        private void MouseLeave()
        {
            isEditing = false;
            this.StateHasChanged();
        }

        private async Task StopEdit()
        {
            try
            {
                await ValueChanged.InvokeAsync(this.Value);

                using var scope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(scope);

                if (this.ItemId > 0)
                {
                    await service.UpdateAsync(this.Item, this.ItemId);
                    AffraNotificationService.NotifyItemUpdated();
                }
                else
                {
                    await service.InsertAsync(this.Item);
                    AffraNotificationService.NotifyItemCreated();
                }

                isEditing = false;
                await OnDataUpdate.InvokeAsync(Year);
                this.StateHasChanged();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private string GetStyle()
        {
            return this.IsRequired && this.Value is null
                ? "background-color: #FFFF99;"
                : null;
        }
    }
}
