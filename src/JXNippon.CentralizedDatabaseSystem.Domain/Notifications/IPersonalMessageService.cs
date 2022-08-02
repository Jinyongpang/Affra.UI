using NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Notifications
{
    public interface IPersonalMessageService
    {
        Task MarkAsReadAsync(PersonalMessage personalMessage);
    }
}