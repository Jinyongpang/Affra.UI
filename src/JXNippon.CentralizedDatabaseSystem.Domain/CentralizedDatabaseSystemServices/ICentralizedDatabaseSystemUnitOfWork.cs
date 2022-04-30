using Affra.Core.Domain.UnitOfWorks;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PowerGenerationAndDistributions;

namespace JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices
{
    public interface ICentralizedDatabaseSystemUnitOfWork : IUnitOfWork
    {
        IGenericRepository<DailyPowerGenerationAndDistribution> DailyPowerGenerationAndDistributionRepository { get; }
    }
}