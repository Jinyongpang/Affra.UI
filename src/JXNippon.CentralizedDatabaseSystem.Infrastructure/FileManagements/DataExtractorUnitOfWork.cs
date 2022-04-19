using Affra.Core.Domain.UnitOfWorks;
using Affra.Core.Infrastructure.UnitOfWorks;
using DataExtractorODataService.Default;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.FileManagements
{
    public class DataExtractorUnitOfWork : ODataUnitOfWorkBase, IDataExtractorUnitOfWork
    {
        private IGenericRepository<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File> _fileRepository;

        public DataExtractorUnitOfWork(IODataClientFactory oDataClientFactory, IOptions<DataExtractorConfigurations> dataExtractorConfigurations) : base(oDataClientFactory.CreateClient<Container>(new Uri(dataExtractorConfigurations.Value.Url), nameof(DataExtractorUnitOfWork)))
        {
        }

        public IGenericRepository<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File> FileRepository => _fileRepository ??= this.GetGenericRepository<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File>();

    }
}
