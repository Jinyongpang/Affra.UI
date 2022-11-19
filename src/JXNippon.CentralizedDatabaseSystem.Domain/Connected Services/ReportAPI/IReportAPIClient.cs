namespace JXNippon.CentralizedDatabaseSystem.Domain.ReportAPI
{
    public partial interface IReportAPIClient
    {
        System.Threading.Tasks.Task<System.Guid> GenerateCombinedDailyReportAsync(string templateFile, CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport combinedDailyReport, System.Threading.CancellationToken cancellationToken);
        Task<Guid> GeneratePEReportAsync(string templateFile, CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports.PEReport peReport, CancellationToken cancellationToken);
    }
}