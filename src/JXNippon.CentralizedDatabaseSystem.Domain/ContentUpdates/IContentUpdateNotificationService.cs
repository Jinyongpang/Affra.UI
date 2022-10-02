
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;

namespace JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates
{
    public interface IContentUpdateNotificationService
    {
        Task RemoveHandlerAsync(string exchangeName, Func<object, Task> handler);
        IHubSubscription Subscribe<T>(string exchangeName, Func<T, Task> handler, CancellationToken cancellationToken = default);
        Task SubscribeAsync(string exchangeName, Func<object, Task> handler, CancellationToken cancellationToken = default);
    }
}