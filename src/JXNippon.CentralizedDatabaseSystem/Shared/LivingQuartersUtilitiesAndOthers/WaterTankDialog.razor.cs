using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.LivingQuartersUtilitiesAndOthers
{
    public partial class WaterTankDialog
    {
        [Parameter] public DailyWaterTank Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected Task SubmitAsync(DailyWaterTank arg)
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
