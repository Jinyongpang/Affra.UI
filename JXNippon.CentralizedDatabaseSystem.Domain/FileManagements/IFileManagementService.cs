
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.FileManagements
{
    public interface IFileManagementService
    {
        DataServiceQuery<Affra.Service.DataExtractor.Domain.Files.File> Get(bool includeCount = true);
    }
}