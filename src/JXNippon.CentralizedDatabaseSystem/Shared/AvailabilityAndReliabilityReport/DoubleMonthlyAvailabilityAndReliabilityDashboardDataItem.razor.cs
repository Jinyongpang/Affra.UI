using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class DoubleMonthlyAvailabilityAndReliabilityDashboardDataItem
    {
        [Parameter] public double? Percentage { get; set; }
    }
}
