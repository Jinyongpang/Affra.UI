using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Reports
{
    public interface IReportService
    {
        Task<Stream> GenerateCombinedDailyReportReportAsync(CombinedDailyReport combinedDailyReport, CancellationToken cancellationToken = default);
    }
}