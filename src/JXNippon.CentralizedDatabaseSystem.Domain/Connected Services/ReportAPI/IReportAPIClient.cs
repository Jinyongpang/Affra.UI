namespace JXNippon.CentralizedDatabaseSystem.Domain.ReportAPI
{
    public partial interface IReportAPIClient
    {
        System.Threading.Tasks.Task<System.Guid> GenerateCombinedDailyReportReportAsync(string templateFile, CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport combinedDailyReport, System.Threading.CancellationToken cancellationToken);
    } 
}