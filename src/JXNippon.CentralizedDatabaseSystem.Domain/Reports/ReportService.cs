using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Domain.ReportAPI;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Workspaces
{
    public class ReportService : IReportService
    {
        private const string CombinedDailyReportTemplate = "CombinedDailyReportTemplate.xlsx";
        private const string PEReportTemplate = "PEReportTemplate.xlsx";
        private readonly IReportAPIClient apiClient;
        private readonly IWorkspaceService _workspaceService;
        public ReportService(IReportAPIClient apiClient, IWorkspaceService workspaceService)
        {
            this.apiClient = apiClient;
            this._workspaceService = workspaceService;
        }

        public async Task<Stream> GenerateCombinedDailyReportAsync(CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport combinedDailyReport, CancellationToken cancellationToken = default)
        {
            var fileId = await this.apiClient.GenerateCombinedDailyReportAsync(CombinedDailyReportTemplate, combinedDailyReport, cancellationToken);
            var fileRespponse = await this._workspaceService.DownloadAsync(fileId, cancellationToken);
            return fileRespponse.Stream;
        }

        public async Task<Stream> GeneratePEReportAsync(CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports.PEReport peReport, CancellationToken cancellationToken = default)
        {
            var fileId = await this.apiClient.GeneratePEReportAsync(PEReportTemplate, peReport, cancellationToken);
            var fileRespponse = await this._workspaceService.DownloadAsync(fileId, cancellationToken);
            return fileRespponse.Stream;
        }
    }
}
