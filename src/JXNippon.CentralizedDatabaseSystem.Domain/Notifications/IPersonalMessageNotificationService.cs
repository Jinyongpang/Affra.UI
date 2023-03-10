using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Notifications
{
    public interface IPersonalMessageNotificationService
    {
        IHubSubscription Subscribe(Func<Message, Task> handler, CancellationToken cancellationToken = default);
    }
}