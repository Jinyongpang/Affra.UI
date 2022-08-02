using Affra.Core.Domain.Services;
using NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Notifications
{
    public class PersonalMessageService : IPersonalMessageService
    {
        private readonly IGenericService<PersonalMessage> genericService;
        private readonly INotificationUnitOfWork notificationUnitOfWork;
        public PersonalMessageService(IUnitGenericService<PersonalMessage, INotificationUnitOfWork> genericService, INotificationUnitOfWork notificationUnitOfWork)
        {
            this.genericService = genericService;
            this.notificationUnitOfWork = notificationUnitOfWork;
        }

        public Task MarkAsReadAsync(PersonalMessage personalMessage)
        {
            personalMessage.Status = PersonalMessageStatus.Read;
            personalMessage.ReadDateTime = DateTime.UtcNow;
            return this.genericService.UpdateAsync(personalMessage, personalMessage.Id);
        }
    }
}
