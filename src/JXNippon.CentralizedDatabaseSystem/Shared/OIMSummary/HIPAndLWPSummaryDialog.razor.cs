using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries;
using JXNippon.CentralizedDatabaseSystem.Shared.TemplateManagement;
using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.OIMSummary
{
    public partial class HIPAndLWPSummaryDialog
    {
        private ExtraColumnEditor extraColumnEditor;
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }
        [Parameter] public DailyHIPAndLWPSummary Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected Task SubmitAsync(DailyHIPAndLWPSummary arg)
        {
            arg.Extras = extraColumnEditor.GetExtras();
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
