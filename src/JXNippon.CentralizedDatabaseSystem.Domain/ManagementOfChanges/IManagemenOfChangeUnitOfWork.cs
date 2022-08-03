using Affra.Core.Domain.UnitOfWorks;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;

namespace JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChange
{
    public interface IManagemenOfChangeUnitOfWork : IUnitOfWork
    {
        IGenericRepository<ManagementOfChangeRecord> ManagementOfChangeRecordRepository { get; }
    }
}