using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.AvailabilityAndReliabilityReport;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class AvailabilityAndReliabilityReportDialog
    {
        [Parameter] public AvailabilityAndReliability Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }

        private bool isViewing { get => MenuAction == 3; }
        protected Task SubmitAsync(AvailabilityAndReliability arg)
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
