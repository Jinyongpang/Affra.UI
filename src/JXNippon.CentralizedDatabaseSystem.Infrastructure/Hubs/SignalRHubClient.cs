using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Hubs
{
    public class SignalRHubClient<T> : IHubClient<T>  where T : HubClientConfigurationsBase
    {
        private IHubConnectionBuilder _hubConnectionBuilder;
        private HubConnection _hubConnection;
        private readonly T hubClientConfigurations;
        private readonly IAccessTokenProvider accessTokenProvider;
        private object lockObject = new object();

        public SignalRHubClient(IOptions<T> hubClientConfigurations, IAccessTokenProvider accessTokenProvider)
        {
            this.hubClientConfigurations = hubClientConfigurations.Value;
            this.accessTokenProvider = accessTokenProvider;
        }

        public IHubSubscription Subscribe<T1>(ICollection<string> methodNames, Func<T1, Task> handler, CancellationToken cancellationToken = default)
        {
            lock (lockObject)
            {
                _hubConnectionBuilder ??= new HubConnectionBuilder()
                .WithUrl(hubClientConfigurations.Url, options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var accessTokenResult = await this.accessTokenProvider.RequestAccessToken();
                        accessTokenResult.TryGetToken(out var accessToken);
                        return accessToken.Value;
                    };

                })
                .WithAutomaticReconnect()
                .AddJsonProtocol();

                _hubConnection ??= _hubConnectionBuilder.Build();
                foreach (var method in methodNames)
                {
                    _hubConnection.On(method, handler);
                }
                IHubSubscription hubScription = new SignalRHubSubscription(_hubConnection);
                return hubScription;
            }          
        }
    }
}
