using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.FileManagements
{
    public class FileManagementService : IFileManagementService
    {
        private readonly IDataExtractorUnitOfWork _dataExtractorUnitOfWork;

        public FileManagementService(IDataExtractorUnitOfWork dataExtractorUnitOfWork)
        {
            _dataExtractorUnitOfWork = dataExtractorUnitOfWork;
        }

        public DataServiceQuery<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File> Get(bool includeCount = true)
        {
            var query = _dataExtractorUnitOfWork.FileRepository.Get() as DataServiceQuery<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File>;
            query = query.IncludeCount(includeCount);
            return query;
        }
    }
}
