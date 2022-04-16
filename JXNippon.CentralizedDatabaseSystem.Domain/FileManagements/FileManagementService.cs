using Affra.Service.DataExtractor;
using Microsoft.OData.Extensions.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.FileManagements
{
    public  class FileManagementService : IFileManagementService
    {
        private readonly IODataClientFactory _clientFactory;

        public FileManagementService(IODataClientFactory oDataClientFactory)
        {
            _clientFactory = oDataClientFactory;
        }

        public async Task<IEnumerable<Affra.Service.DataExtractor.Domain.Files.File>> GetAsync()
        {
            var client = _clientFactory.CreateClient<Container>(new Uri("http://localhost:7200/DataExtractor-api/odata/$metadata"), nameof(FileManagementService));
            client.HttpRequestTransportMode = Microsoft.OData.Client.HttpRequestTransportMode.HttpClient;
            return await client.File.ExecuteAsync();
        }
    }
}
