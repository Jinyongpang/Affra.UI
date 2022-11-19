using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Domain.ReportAPI;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Workspaces
{
    public class ReportService : IReportService
    {
        private const string CombinedDailyReportTemplate = "CombinedDailyReportTemplate.xlsx";
        private const string PEReportTemplate = "PEReportTemplate.xlsx";
        private readonly IReportAPIClient apiClient;
        private readonly IWorkspaceService _workspaceService;
        private readonly IReportUnitOfWork _reportUnitOfWork;
        public ReportService(IReportAPIClient apiClient, IWorkspaceService workspaceService, IReportUnitOfWork reportUnitOfWork)
        {
            this.apiClient = apiClient;
            this._workspaceService = workspaceService;
            this._reportUnitOfWork = reportUnitOfWork;
        }

        public async Task<Guid> GenerateCombinedDailyReportAsync(CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport combinedDailyReport, CancellationToken cancellationToken = default)
        {
            var referenceId = await this.apiClient.GenerateCombinedDailyReportAsync(CombinedDailyReportTemplate, combinedDailyReport, cancellationToken);
            return referenceId;
        }

        public async Task<Guid> GeneratePEReportAsync(CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports.PEReport peReport, CancellationToken cancellationToken = default)
        {
            var referenceId = await this.apiClient.GeneratePEReportAsync(PEReportTemplate, peReport, cancellationToken);
            return referenceId;
        }
        public async Task<Stream> DownloadReportAsync(Guid referenceId, CancellationToken cancellationToken = default)
        {
            var receipt = (await ((DataServiceQuery<ReportODataService.Affra.Service.Report.Domain.Reports.ReportReceipt>)this._reportUnitOfWork.ReportReceiptRepository.Get()
                .Where(x => x.ReferenceId == referenceId))
                .ExecuteAsync())
                .FirstOrDefault();
            if (receipt == null
                || receipt.Status == ReportODataService.Affra.Service.Report.Domain.Reports.ReportGenerationStatus.Failed)
            {
                throw new InvalidOperationException("Generating report failed!");
            }
            else if(receipt.Status == ReportODataService.Affra.Service.Report.Domain.Reports.ReportGenerationStatus.InProgress)
            {
                throw new InvalidOperationException("Generating report is still in progress.");
            }

            var fileRespponse = await this._workspaceService.DownloadAsync(receipt.FileId, cancellationToken);
            return fileRespponse.Stream;
        }
    }
}
