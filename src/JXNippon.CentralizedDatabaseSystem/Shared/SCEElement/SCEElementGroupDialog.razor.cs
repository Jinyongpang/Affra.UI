using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.SCEElements;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.SCEElement
{
    public partial class SCEElementGroupDialog
    {
        [Parameter] public SCEElementGroupRecord Item { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        protected Task SubmitAsync(SCEElementGroupRecord arg)
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
