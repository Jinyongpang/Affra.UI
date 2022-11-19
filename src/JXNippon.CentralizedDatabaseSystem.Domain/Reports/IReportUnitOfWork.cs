using Affra.Core.Domain.UnitOfWorks;
using ReportODataService.Affra.Service.Report.Domain.Reports;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Reports
{
    public interface IReportUnitOfWork : IUnitOfWork
    {
        IGenericRepository<ReportReceipt> ReportReceiptRepository { get; }
    }
}