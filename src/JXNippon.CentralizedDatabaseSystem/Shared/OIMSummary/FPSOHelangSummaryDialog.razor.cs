using Microsoft.AspNetCore.Components;
using Radzen;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries;

namespace JXNippon.CentralizedDatabaseSystem.Shared.OIMSummary
{
    public partial class FPSOHelangSummaryDialog
    {
        [Parameter] public DailyFPSOHelangSummary Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected Task SubmitAsync(DailyFPSOHelangSummary arg)
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
