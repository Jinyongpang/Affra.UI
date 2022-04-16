
namespace JXNippon.CentralizedDatabaseSystem.Domain.FileManagements
{
    public interface IFileManagementService
    {
        Task<IEnumerable<Affra.Service.DataExtractor.Domain.Files.File>> GetAsync();
    }
}