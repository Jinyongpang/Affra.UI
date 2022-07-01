
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;

namespace JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates
{
    public interface IContentUpdateNotificationService
    {
        IHubSubscription Subscribe<T>(string exchangeName, Func<T, Task> handler, CancellationToken cancellationToken = default);
    }
}