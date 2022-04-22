using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
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

        public DataServiceQuery<DataFile> Get(bool includeCount = true)
        {
            var query = _dataExtractorUnitOfWork.DataFileRepository.Get() as DataServiceQuery<DataFile>;
            query = query.IncludeCount(includeCount);
            return query;
        }
    }
}
