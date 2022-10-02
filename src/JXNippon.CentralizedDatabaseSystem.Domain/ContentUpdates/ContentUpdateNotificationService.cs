using System.Collections.Concurrent;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;

namespace JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates
{
    public class ContentUpdateNotificationService : IContentUpdateNotificationService
    {
        private readonly IHubClient<ContentUpdateNotificationServiceConfigurations> hubClient;

        private static readonly IDictionary<string, ContentUpdateHandler> contentUpdateHandlers = new ConcurrentDictionary<string, ContentUpdateHandler>();

        private static readonly IDictionary<string, IHubSubscription> hubSubscriptions = new ConcurrentDictionary<string, IHubSubscription>();

        public ContentUpdateNotificationService(IHubClient<ContentUpdateNotificationServiceConfigurations> hubClient)
        {
            this.hubClient = hubClient;
        }

        public IHubSubscription Subscribe<T>(string exchangeName, Func<T, Task> handler, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.hubClient.Subscribe(new[] { exchangeName, $"{exchangeName}.Deleted" }, handler, cancellationToken);
        }

        public async Task SubscribeAsync(string exchangeName, Func<object, Task> handler, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!contentUpdateHandlers.TryGetValue(exchangeName, out var contentUpdateHandler))
            {
                contentUpdateHandler = new ContentUpdateHandler();
                contentUpdateHandlers.Add(exchangeName, contentUpdateHandler);
            }

            contentUpdateHandler.Handler += handler;
            if (!hubSubscriptions.TryGetValue(exchangeName, out var subscription))
            {
                subscription = this.hubClient.Subscribe<object>(new[] { exchangeName, $"{exchangeName}.Deleted" }, contentUpdateHandler.OnContentUpdateAsync, cancellationToken);
                hubSubscriptions.Add(exchangeName, subscription);
                await subscription.StartAsync();
            }
        }

        public async Task RemoveHandlerAsync(string exchangeName, Func<object, Task> handler)
        {
            if (contentUpdateHandlers.TryGetValue(exchangeName, out var contentUpdateHandler))
            {
                contentUpdateHandler.Handler -= handler;

                if (contentUpdateHandler.Handler is null)
                {
                    contentUpdateHandlers.Remove(exchangeName);
                    if (hubSubscriptions.TryGetValue(exchangeName, out var subscription))
                    {
                        hubSubscriptions.Remove(exchangeName);
                        await subscription.DisposeAsync();
                    }
                }
            }
        }
    }
}

