using Microsoft.Extensions.Options;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Workspaces
{
    public class WorkspaceAPIClient : Domain.WorkspaceAPI.WorkspaceAPIClient
    {
        public WorkspaceAPIClient(HttpClient httpClient, IOptions<WorkspaceAPIConfigurations> workspaceAPIConfigurations) : base(httpClient)
        {
            BaseUrl = workspaceAPIConfigurations.Value.Url;
        }
    }
}
