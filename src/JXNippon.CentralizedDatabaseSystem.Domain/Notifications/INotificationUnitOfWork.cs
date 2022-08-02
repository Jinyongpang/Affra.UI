using Affra.Core.Domain.UnitOfWorks;
using NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Notifications
{
    public interface INotificationUnitOfWork : IUnitOfWork
    {
        IGenericRepository<PersonalMessage> PersonalMessageRepository { get; }
    }
}