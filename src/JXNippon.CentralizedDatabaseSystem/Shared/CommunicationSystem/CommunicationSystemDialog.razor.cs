using Microsoft.AspNetCore.Components;
using Radzen;
using CommunicationSystems = CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CommunicationSystems;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem
{
    public partial class CommunicationSystemDialog
    {
        [Parameter] public CommunicationSystems.DailyCommunicationSystem Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected Task SubmitAsync(CommunicationSystems.DailyCommunicationSystem arg)
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
