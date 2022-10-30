using JXNippon.CentralizedDatabaseSystem.Domain.WorkspaceAPI;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Workspaces
{
    public interface IWorkspaceService
    {
        Task<FileResponse> DownloadAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> GetIdAsync(string workspaceName, string fileName, CancellationToken cancellationToken = default);
        Task<WorkspaceFile> UploadAsync(string workspace, FileParameter file, CancellationToken cancellationToken = default);
    }
}