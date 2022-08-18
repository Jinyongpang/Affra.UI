using Affra.Core.Domain.UnitOfWorks;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;

namespace JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges
{
    public interface IManagementOfChangeUnitOfWork : IUnitOfWork
    {
        IGenericRepository<ManagementOfChangeRecord> ManagementOfChangeRecordRepository { get; }
    }
}