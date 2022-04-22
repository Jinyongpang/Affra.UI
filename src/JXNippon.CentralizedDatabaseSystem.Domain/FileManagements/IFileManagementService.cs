using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.FileManagements
{
    public interface IFileManagementService
    {
        DataServiceQuery<DataFile> Get(bool includeCount = true);
    }
}