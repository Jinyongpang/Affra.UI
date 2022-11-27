using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport;
using JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class AvailabilityAndReliabilityReportManagement
    {
        private IDateFilterComponent dateFilterComponent;
        private AvailabilityAndReliabilityReportGrid availabilityAndReliabilityReportGrid { get; set; }
        private async Task ReloadAsync()
        {
            availabilityAndReliabilityReportGrid.Filter.DateRange = new DateRange { Start = dateFilterComponent?.Start, End = dateFilterComponent?.End };
            await availabilityAndReliabilityReportGrid.ReloadAsync();
            this.StateHasChanged();
        }
    }
}
