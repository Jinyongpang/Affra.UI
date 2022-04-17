using Affra.Core.Domain.UnitOfWorks;

namespace JXNippon.CentralizedDatabaseSystem.Domain.FileManagements
{
    public interface IDataExtractorUnitOfWork : IUnitOfWork
    {
        IGenericRepository<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File> FileRepository { get; }
    }
}