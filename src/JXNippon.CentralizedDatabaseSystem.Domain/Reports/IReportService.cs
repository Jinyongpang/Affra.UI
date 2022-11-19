using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Reports
{
    public interface IReportService
    {
        Task<Stream> DownloadReportAsync(Guid referenceId, CancellationToken cancellationToken = default);
        Task<Guid> GenerateCombinedDailyReportAsync(CombinedDailyReport combinedDailyReport, CancellationToken cancellationToken = default);
        Task<Guid> GeneratePEReportAsync(PEReport peReport, CancellationToken cancellationToken = default);
    }
}