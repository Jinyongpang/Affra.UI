using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Hubs
{
    public class SignalRHubClient<T> : IHubClient<T>  where T : HubClientConfigurationsBase
    {
        private readonly T hubClientConfigurations;

        public SignalRHubClient(IOptions<T> hubClientConfigurations)
        {
            this.hubClientConfigurations = hubClientConfigurations.Value;

        }

        public IHubSubscription Subscribe<T1>(ICollection<string> methodNames, Func<T1, Task> handler, CancellationToken cancellationToken = default)
        {
            var hubConnectionBuilder = new HubConnectionBuilder()
                .WithUrl(hubClientConfigurations.Url)
                .WithAutomaticReconnect()
                .AddJsonProtocol();
            HubConnection hubConnection = hubConnectionBuilder.Build();
            foreach(var method in methodNames)
            {
                hubConnection.On(method, handler);
            }
            IHubSubscription hubScription = new SignalRHubSubscription(hubConnection);
            return hubScription;
        }
    }
}
