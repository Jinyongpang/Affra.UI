using Affra.Core.Domain.UnitOfWorks;
using Affra.Core.Infrastructure.OData.UnitOfWorks;
using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.Reports;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;
using ReportODataService.Affra.Service.Report.Domain.Reports;
using ReportODataService.Default;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.ManagementOfChanges
{
    public class ReportUnitOfWork : ODataUnitOfWorkBase, IReportUnitOfWork
    {
        private IGenericRepository<ReportReceipt> _reportReceiptRepository;
        public ReportUnitOfWork(IODataClientFactory oDataClientFactory, IOptions<ReportConfigurations> configurations)
           : base(oDataClientFactory.CreateClient<Container>(new Uri(configurations.Value.Url), nameof(ReportUnitOfWork)))
        {
        }

        public IGenericRepository<ReportReceipt> ReportReceiptRepository => _reportReceiptRepository ??= GetGenericRepository<ReportReceipt>();
    }
}