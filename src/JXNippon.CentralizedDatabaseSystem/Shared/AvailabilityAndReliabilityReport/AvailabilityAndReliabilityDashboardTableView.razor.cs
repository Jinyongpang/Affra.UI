using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class AvailabilityAndReliabilityDashboardTableView
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public int Year { get; set; }
        [Parameter] public string ReportName { get; set; }
    }
}
