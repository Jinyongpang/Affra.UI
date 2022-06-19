using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.HealthSafetyAndEnvironment
{
    public partial class LossOfPrimaryContainmentIncidentDialog
    {
        [Parameter] public DailyLossOfPrimaryContainmentIncident Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected Task SubmitAsync(DailyLossOfPrimaryContainmentIncident arg)
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
