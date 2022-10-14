using Affra.Core.Infrastructure.OData.Models;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.Extensions.Localization;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Notifications
{
    public class AffraNotificationService
    {
        private const int NotificationDuration = 5;
        private readonly NotificationService notificationService;
        private readonly AntDesign.NotificationService antNotification;
        private readonly IStringLocalizer<Resource> stringLocalizer;
        private readonly IGlobalDataSource globalDataSource;

        public AffraNotificationService(NotificationService notificationService, 
            IStringLocalizer<Resource> stringLocalizer, 
            IGlobalDataSource globalDataSource,
            AntDesign.NotificationService notification)
        { 
            this.notificationService = notificationService;
            this.stringLocalizer = stringLocalizer;
            this.globalDataSource = globalDataSource;
            this.antNotification = notification;
        }

        public void NotifyException(Exception exception)
        {
            this.globalDataSource.AddException(exception);
            if (exception is AffraODataException odataEx)
            {
                _ = antNotification.Error(new()
                {
                    Message = odataEx.Message,
                    Description = stringLocalizer[odataEx.AffraProblemDetails.ErrorCode.ToString()].ToString(),
                    Duration = NotificationDuration,
                });
            }
            else
            {
                _ = antNotification.Error(new()
                {
                    Message = exception.Message,
                });
            }
        }

        public void NotifyItemCreated()
        {
            this.NoticeWithIcon(AntDesign.NotificationType.Success, stringLocalizer["New item added."]);
        }

        public void NotifyItemUpdated()
        {
            this.NoticeWithIcon(AntDesign.NotificationType.Success, stringLocalizer["Item updated."]);
        }

        public void NotifyItemDeleted()
        {
            this.NoticeWithIcon(AntDesign.NotificationType.Success, stringLocalizer["Item deleted."]);
        }

        public void NotifyWarning(string message, string detail= default)
        {
            this.NoticeWithIcon(AntDesign.NotificationType.Warning, stringLocalizer[message], detail);
        }

        public void NotifyInfo(string message, string detail = default)
        {
            this.NoticeWithIcon(AntDesign.NotificationType.Info, stringLocalizer[message], detail);
        }

        public void NotifyError(string message, string detail = default)
        {
            this.NoticeWithIcon(AntDesign.NotificationType.Error, stringLocalizer[message], detail);
        }

        public void NotifySuccess(string message, string detail = default)
        {
            this.NoticeWithIcon(AntDesign.NotificationType.Success, stringLocalizer[message], detail);
        }

        private void NoticeWithIcon(AntDesign.NotificationType type, string message, string detail = default)
        {
            _ = antNotification.Open(new ()
            {
                Message = message,
                Description = detail,
                NotificationType = type,
                Duration = NotificationDuration,
            });
        }

    }
}
