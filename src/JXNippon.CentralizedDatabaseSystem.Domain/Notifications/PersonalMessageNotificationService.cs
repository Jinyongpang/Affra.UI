using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Notifications
{
    public class PersonalMessageNotificationService : IPersonalMessageNotificationService
    {
        private const string PersonalMessagesQueue = "PersonalMessages";
        private readonly IHubClient<PersonalMessageNotificationServiceConfigurations> hubClient;
        public PersonalMessageNotificationService(IHubClient<PersonalMessageNotificationServiceConfigurations> hubClient)
        {
            this.hubClient = hubClient;
        }

        public IHubSubscription Subscribe(Func<Message, Task> handler, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.hubClient.Subscribe(new[] { PersonalMessagesQueue }, handler, cancellationToken);
        }
    }
}
