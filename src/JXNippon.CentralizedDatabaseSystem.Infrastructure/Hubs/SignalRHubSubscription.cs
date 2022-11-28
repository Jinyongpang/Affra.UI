using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using Microsoft.AspNetCore.SignalR.Client;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Hubs
{
    public class SignalRHubSubscription : IHubSubscription
    {
        private readonly HubConnection hubConnection;
        private bool disposed;
        private bool isStarted;
        private object isStartedLock = new object();

        public SignalRHubSubscription(HubConnection hubConnection)
        { 
            this.hubConnection = hubConnection;
        }

        public event Func<Exception?, Task>? Closed;
        public event Func<Exception?, Task>? Reconnecting;
        public event Func<string?, Task>? Reconnected;

        public async ValueTask DisposeAsync()
        {
            return;
            if (!disposed && hubConnection is not null)
            {
                this.hubConnection.Closed -= Closed;
                this.hubConnection.Reconnecting -= Reconnecting;
                this.hubConnection.Reconnected -= Reconnected;
                await hubConnection.DisposeAsync();
                disposed = true;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {       
            bool isStartedInner = false;
            lock (isStartedLock)
            {
                isStartedInner = this.isStarted;
                if (!isStarted)
                {
                    this.hubConnection.Closed += Closed;
                    this.hubConnection.Reconnecting += Reconnecting;
                    this.hubConnection.Reconnected += Reconnected;
                    this.isStarted = true;
                }
            }
            try
            {
                if (!isStartedInner)
                {
                    await this.hubConnection.StartAsync(cancellationToken);
                }
            }
            catch
            {
            }
        }
    }
}
