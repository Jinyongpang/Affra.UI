using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Domain.ReportAPI;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Workspaces
{
    public class ReportService : IReportService
    {
        private const string CombinedDailyReportTemplate = "CombinedDailyReportTemplate.xlsx";
        private readonly IReportAPIClient apiClient;
        private readonly IWorkspaceService _workspaceService;
        public ReportService(IReportAPIClient apiClient, IWorkspaceService workspaceService)
        {
            this.apiClient = apiClient;
            this._workspaceService = workspaceService;
        }

        public async Task<Stream> GenerateCombinedDailyReportReportAsync(CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport combinedDailyReport, CancellationToken cancellationToken = default)
        {
            var fileId = await this.apiClient.GenerateCombinedDailyReportReportAsync(CombinedDailyReportTemplate, combinedDailyReport, cancellationToken);
            var fileRespponse = await this._workspaceService.DownloadAsync(fileId, cancellationToken);
            return fileRespponse.Stream;
        }
    }
}
