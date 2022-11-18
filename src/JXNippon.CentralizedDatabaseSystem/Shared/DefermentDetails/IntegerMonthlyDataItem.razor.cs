using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class IntegerMonthlyDataItem<TItem>
        where TItem : class
    {
        [Parameter] public int Value { get; set; }
        [Parameter] public TItem Item { get; set; }
        [Parameter] public long ItemId { get; set; }
        [Parameter] public bool IsEditable { get; set; } = false;
        [Parameter] public bool IsRequired { get; set; } = false;
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Parameter] public EventCallback<int> ValueChanged { get; set; }

        private IGenericService<TItem> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private bool isEditing;

        private async Task StartEdit()
        {
            if (IsEditable)
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
                this.StateHasChanged();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private string GetStyle()
        {
            return this.IsRequired && this.Value == null
                ? "background-color: #FFFF99;"
                : null;
        }
    }
}
