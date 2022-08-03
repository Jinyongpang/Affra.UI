using Affra.Core.Domain.UnitOfWorks;
using Affra.Core.Infrastructure.OData.UnitOfWorks;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChange;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using ManagementOfChangeODataService.Default;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.ManagementOfChanges
{
    public class ManagemenOfChangeUnitOfWork : ODataUnitOfWorkBase, IManagemenOfChangeUnitOfWork
    {
        private IGenericRepository<ManagementOfChangeRecord> _managementOfChangeRecordRepository;
        public ManagemenOfChangeUnitOfWork(IODataClientFactory oDataClientFactory, IOptions<ManagementOfChangeConfigurations> configurations)
           : base(oDataClientFactory.CreateClient<Container>(new Uri(configurations.Value.Url), nameof(ManagemenOfChangeUnitOfWork)))
        {
        }

        public IGenericRepository<ManagementOfChangeRecord> ManagementOfChangeRecordRepository => _managementOfChangeRecordRepository ??= this.GetGenericRepository<ManagementOfChangeRecord>();
    }
}