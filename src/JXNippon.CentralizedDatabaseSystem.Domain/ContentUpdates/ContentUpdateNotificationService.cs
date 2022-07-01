using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;

namespace JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates
{
    public class ContentUpdateNotificationService : IContentUpdateNotificationService
    {
        private readonly IHubClient<ContentUpdateNotificationServiceConfigurations> hubClient;
        public ContentUpdateNotificationService(IHubClient<ContentUpdateNotificationServiceConfigurations> hubClient)
        {
            this.hubClient = hubClient;
        }

        public IHubSubscription Subscribe<T>(string exchangeName, Func<T, Task> handler, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.hubClient.Subscribe(new[] { exchangeName, $"{exchangeName}.Deleted" }, handler, cancellationToken);
        }
    }
}
