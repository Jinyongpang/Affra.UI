using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class MOCFormTemplate
    {
        [Parameter] public ManagementOfChangeRecord Item { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }

        private int currentStep;

        private void OnNextButtonClick()
        {
            currentStep++;
        }
        private void OnPreviousButtonClick()
        {
            currentStep--;
        }
        private void OnStepChange(int current)
        {
            this.currentStep = current;
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
