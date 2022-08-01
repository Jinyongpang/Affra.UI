using Affra.Core.Domain.UnitOfWorks;
using Affra.Core.Infrastructure.OData.UnitOfWorks;
using JXNippon.CentralizedDatabaseSystem.Domain.Notifications;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;
using NotficationODataService.Affra.Service.Notification.Domain.PersonalMessages;
using NotficationODataService.Default;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Notifications
{
    public class NotificationUnitOfWork : ODataUnitOfWorkBase, INotificationUnitOfWork
    {
        private IGenericRepository<PersonalMessage> _personalMessageRepository;
        public NotificationUnitOfWork(IODataClientFactory oDataClientFactory, IOptions<NotificationConfigurations> configurations)
           : base(oDataClientFactory.CreateClient<Container>(new Uri(configurations.Value.Url), nameof(NotificationUnitOfWork)))
        {
        }

        public IGenericRepository<PersonalMessage> PersonalMessageRepository => _personalMessageRepository ??= this.GetGenericRepository<PersonalMessage>();
    }
}