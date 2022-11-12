using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Reports
{
    public interface IReportService
    {
        Task<Stream> GenerateCombinedDailyReportAsync(CombinedDailyReport combinedDailyReport, CancellationToken cancellationToken = default);
        Task<Stream> GeneratePEReportAsync(PEReport peReport, CancellationToken cancellationToken = default);
    }
}