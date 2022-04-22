using Affra.Core.Domain.UnitOfWorks;
using Affra.Core.Infrastructure.UnitOfWorks;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using DataExtractorODataService.Default;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.FileManagements
{
    public class DataExtractorUnitOfWork : ODataUnitOfWorkBase, IDataExtractorUnitOfWork
    {
        private IGenericRepository<DataFile> _fileRepository;

        public DataExtractorUnitOfWork(IODataClientFactory oDataClientFactory, IOptions<DataExtractorConfigurations> dataExtractorConfigurations) : base(oDataClientFactory.CreateClient<Container>(new Uri(dataExtractorConfigurations.Value.Url), nameof(DataExtractorUnitOfWork)))
        {
        }

        public IGenericRepository<DataFile> DataFileRepository => _fileRepository ??= this.GetGenericRepository<DataFile>();

    }
}
