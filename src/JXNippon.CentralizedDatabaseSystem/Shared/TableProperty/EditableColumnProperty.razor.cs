using System.Linq.Expressions;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.TableProperty
{
    public partial class EditableColumnProperty<TItem, TProp>
        where TItem : class
    {
        [Parameter] public TItem Item { get; set; }
        [Parameter] public long ItemId { get; set; }
        [Parameter] public Expression<Func<TItem, TProp>> Property { get; set; }
        [Parameter] public TProp Value { get; set; }
        [Parameter] public EventCallback<TProp> ValueChanged { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        private IGenericService<TItem> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private bool isEditing;

        private void startEdit()
        {
            isEditing = true;
        }

        private async Task stopEdit()
        {
            try
            {
                using var scope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(scope);
                await service.UpdateAsync(Item, ItemId);

                this.AffraNotificationService.NotifySuccess("Record edited.");

                isEditing = false;
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }
    }
}
