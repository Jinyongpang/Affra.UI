using Microsoft.OData.Client;

namespace DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFolders
{
    public partial class Folder
    {
        [IgnoreClientProperty]
        public string Section { get; set; }
    }
}