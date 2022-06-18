using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MaximoWorkOrders;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.MaximoWorkOrder
{
    public partial class MaximoWorkOrderDialog
    {
        [Parameter] public DailyMaximoWorkOrder Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected Task SubmitAsync(DailyMaximoWorkOrder arg)
        {
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
