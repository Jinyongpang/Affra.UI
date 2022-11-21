using JXNippon.CentralizedDatabaseSystem.Handlers;
using Microsoft.OData.Extensions.Client;

namespace JXNippon.CentralizedDatabaseSystem.Extensions
{
    public static class ODataClientExtensions
    {
        public static IServiceCollection AddODataHttpClient(this IServiceCollection services, string name)
        {
            return services.AddODataClient(name)
                .AddHttpClient()
                .AddHttpMessageHandler<CreateActivityHandler>()
                .AddHttpMessageHandler<ODataEnumHandler>()
                .AddHttpMessageHandler<AuthorizationMessageHandler>()
                .Services;
        }
    }
}
