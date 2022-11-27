using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportApprovalConfirmationDialog
	{
        [Inject] private DialogService DialogService { get; set; }
        [Parameter] public int UniformanceErrorCount { get; set; }
        [Parameter] public int UniformanceNotInToleranceCount { get; set; }
        private void ConfirmClicked() => DialogService.Close(true);
        private void CancelClicked() => DialogService.Close(false);

        private string GetTitle()
        {
            return $"This report have {UniformanceErrorCount} uniformance error and {UniformanceNotInToleranceCount} not within tolerance. Are you sure to approve?";
        }
    }
}
