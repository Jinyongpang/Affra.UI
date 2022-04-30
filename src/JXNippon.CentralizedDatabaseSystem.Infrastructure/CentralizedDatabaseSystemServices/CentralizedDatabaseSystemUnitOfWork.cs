using Affra.Core.Domain.UnitOfWorks;
using Affra.Core.Infrastructure.UnitOfWorks;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PowerGenerationAndDistributions;
using CentralizedDatabaseSystemODataService.Default;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.FileManagements;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.CentralizedDatabaseSystemServices
{
    public class CentralizedDatabaseSystemUnitOfWork : ODataUnitOfWorkBase, ICentralizedDatabaseSystemUnitOfWork
    {
        private IGenericRepository<DailyPowerGenerationAndDistribution> _dailyPowerGenerationAndDistributionRepository;

        public CentralizedDatabaseSystemUnitOfWork(IODataClientFactory oDataClientFactory, IOptions<CentralizedDatabaseSystemConfigurations> centralizedDatabaseSystemConfigurations) : base(oDataClientFactory.CreateClient<Container>(new Uri(centralizedDatabaseSystemConfigurations.Value.Url), nameof(DataExtractorUnitOfWork)))
        {
        }

        public IGenericRepository<DailyPowerGenerationAndDistribution> DailyPowerGenerationAndDistributionRepository => _dailyPowerGenerationAndDistributionRepository ??= this.GetGenericRepository<DailyPowerGenerationAndDistribution>();

    }
}