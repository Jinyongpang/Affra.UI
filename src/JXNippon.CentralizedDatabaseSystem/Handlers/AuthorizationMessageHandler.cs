using JXNippon.CentralizedDatabaseSystem.Configurations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace JXNippon.CentralizedDatabaseSystem.Handlers
{
    public class AuthorizationMessageHandler : Microsoft.AspNetCore.Components.WebAssembly.Authentication.AuthorizationMessageHandler
    {
        public AuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, IOptions<AzureAdConfigurations> azureAdConfigurations)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: azureAdConfigurations.Value.AuthorizedUrls,
                scopes: azureAdConfigurations.Value.Scopes);
        }
    }
}
