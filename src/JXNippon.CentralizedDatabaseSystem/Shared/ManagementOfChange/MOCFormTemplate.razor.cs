using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class MOCFormTemplate
    {
        [Parameter] public ManagementOfChangeRecord Item { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private int CurrentStep;

        private void OnNextButtonClick()
        {
            CurrentStep++;
        }
        private void OnPreviousButtonClick()
        {
            CurrentStep--;
        }
        private void OnStepChange(int current)
        {
            this.CurrentStep = current;
        }
        protected Task SubmitAsync(ManagementOfChangeRecord args)
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
