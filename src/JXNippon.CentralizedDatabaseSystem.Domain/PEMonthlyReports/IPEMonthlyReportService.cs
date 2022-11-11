using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;

namespace JXNippon.CentralizedDatabaseSystem.Domain.PEMonthlyReports
{
    public interface IPEMonthlyReportService
    {
        Task<PEReport> GetPEMonthlyReportAsync(DateTimeOffset date);
    }
}