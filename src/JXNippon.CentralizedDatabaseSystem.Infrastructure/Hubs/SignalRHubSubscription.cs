using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using Microsoft.AspNetCore.SignalR.Client;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Hubs
{
    public class SignalRHubSubscription : IHubSubscription
    {
        private readonly HubConnection hubConnection;
        private readonly ICollection<IDisposable> handlers;
        private bool disposed;

        public SignalRHubSubscription(HubConnection hubConnection, ICollection<IDisposable> handlers)
        { 
            this.hubConnection = hubConnection;
            this.handlers = handlers;
        }

        public event Func<Exception?, Task>? Closed;
        public event Func<Exception?, Task>? Reconnecting;
        public event Func<string?, Task>? Reconnected;

        public async ValueTask DisposeAsync()
        {
            if (!disposed && hubConnection is not null)
            {
                this.hubConnection.Closed -= Closed;
                this.hubConnection.Reconnecting -= Reconnecting;
                this.hubConnection.Reconnected -= Reconnected;
                foreach(var handler in this.handlers) 
                {
                    handler.Dispose();
                }
                await hubConnection.DisposeAsync();
                disposed = true;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            this.hubConnection.Closed += Closed;
            this.hubConnection.Reconnecting += Reconnecting;
            this.hubConnection.Reconnected += Reconnected;
            return this.hubConnection.StartAsync(cancellationToken);
        }
    }
}
