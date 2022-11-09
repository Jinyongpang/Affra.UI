using System.Collections.ObjectModel;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries;

namespace JXNippon.CentralizedDatabaseSystem.Domain.CombinedDailyReports
{
    public interface ICombinedDailyReportService
    {
        Collection<DailyFPSOHelangSummary> AppendSummary(Collection<DailyFPSOHelangSummary> data, CombinedDailyReport combinedDailyReport);
        Collection<DailyHIPAndLWPSummary> AppendSummary(Collection<DailyHIPAndLWPSummary> data, CombinedDailyReport combinedDailyReport);
        Task<CombinedDailyReport> GetCombinedDailyReportAsync(DateTimeOffset date);
        Task<CombinedDailyReport> GetFullCombinedDailyReportAsync(DateTimeOffset date);
    }
}