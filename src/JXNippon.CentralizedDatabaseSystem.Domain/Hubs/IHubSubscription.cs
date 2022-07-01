namespace JXNippon.CentralizedDatabaseSystem.Domain.Hubs
{
    public interface IHubSubscription : IAsyncDisposable
    {
        event Func<Exception?, Task>? Closed;

        event Func<Exception?, Task>? Reconnecting;

        event Func<string?, Task>? Reconnected;

        Task StartAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
