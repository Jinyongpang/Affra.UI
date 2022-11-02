using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportTag
    {
        [Parameter] public int Count { get; set; }

        private string GetColor()
        {
            return Count == 0
                ? "Green"
                : "Red";
        }

    }
}
