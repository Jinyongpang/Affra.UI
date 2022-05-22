using Microsoft.AspNetCore.Components;
using Radzen;
using LWPActivities = CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.LWPActivities;

namespace JXNippon.CentralizedDatabaseSystem.Shared.LWPActivity
{
    public partial class LWPActivityDialog
    {
        [Parameter] public LWPActivities.DailyLWPActivity Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected Task SubmitAsync(LWPActivities.DailyLWPActivity arg)
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
