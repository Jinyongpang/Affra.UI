using JXNippon.CentralizedDatabaseSystem.Domain.WorkspaceAPI;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Workspaces
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IWorkspaceAPIClient workspaceAPIClient;
        public WorkspaceService(IWorkspaceAPIClient workspaceAPIClient)
        {
            this.workspaceAPIClient = workspaceAPIClient;
        }

        public Task<WorkspaceFile> UploadAsync(string workspace, FileParameter file, CancellationToken cancellationToken = default)
        {
            return workspaceAPIClient.WorkspaceFilePostAsync(workspace, file, cancellationToken);
        }

        public Task<Guid> GetIdAsync(string workspaceName, string fileName, CancellationToken cancellationToken = default)
        {
            return workspaceAPIClient.WorkspaceFileGetAsync(workspaceName, fileName, cancellationToken);
        }

        public Task<FileResponse> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return workspaceAPIClient.WorkspaceFileGetAsync(id, cancellationToken);
        }

    }
}
