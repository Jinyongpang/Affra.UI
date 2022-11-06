using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;

namespace JXNippon.CentralizedDatabaseSystem.Domain.CombinedDailyReports
{
    public interface ICombinedDailyReportService
    {
        Task<CombinedDailyReport> GetCombinedDailyReportAsync(DateTimeOffset date);
    }
}