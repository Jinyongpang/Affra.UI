using Affra.Core.Domain.UnitOfWorks;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;

namespace JXNippon.CentralizedDatabaseSystem.Domain.FileManagements
{
    public interface IDataExtractorUnitOfWork : IUnitOfWork
    {
        IGenericRepository<DataFile> DataFileRepository { get; }
    }
}