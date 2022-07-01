using Affra.Core.Infrastructure.OData.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.Extensions.Localization;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Notifications
{
    public class AffraNotificationService
    {
        private const int NotificationDration = 10000;
        private readonly NotificationService notificationService;
        private readonly IStringLocalizer<Resource> stringLocalizer;

        public AffraNotificationService(NotificationService notificationService, IStringLocalizer<Resource> stringLocalizer)
        { 
            this.notificationService = notificationService;
            this.stringLocalizer = stringLocalizer;
        }

        public void NotifyException(Exception exception)
        {
            if (exception is AffraODataException odataEx)
            {
                notificationService.Notify(new()
                {
                    Summary = odataEx.AffraProblemDetails.Title,
                    Detail = stringLocalizer[odataEx.AffraProblemDetails.ErrorCode.ToString()],
                    Severity = NotificationSeverity.Error,
                    Duration = NotificationDration,
                });
            }
            else
            {
                notificationService.Notify(new()
                {
                    Summary = "Error",
                    Detail = (exception.InnerException ?? exception).ToString(),
                    Severity = NotificationSeverity.Error,
                    Duration = NotificationDration,
                });
            }
        }

        public void NotifyItemCreated()
        {
            notificationService.Notify(new()
            {
                Summary = stringLocalizer["New item added."],
                Detail = string.Empty,
                Severity = NotificationSeverity.Success,
                Duration = NotificationDration,
            });
        }

        public void NotifyItemUpdated()
        {
            notificationService.Notify(new()
            {
                Summary = stringLocalizer["Item updated."],
                Detail = string.Empty,
                Severity = NotificationSeverity.Success,
                Duration = NotificationDration,
            });
        }

        public void NotifyItemDeleted()
        {
            notificationService.Notify(new()
            {
                Summary = stringLocalizer["Item deleted."],
                Detail = string.Empty,
                Severity = NotificationSeverity.Success,
                Duration = NotificationDration,
            });
        }

        public void NotifyWarning(string message, string detail= default)
        {
            notificationService.Notify(new()
            {
                Summary = stringLocalizer[message],
                Detail = detail,
                Severity = NotificationSeverity.Warning,
                Duration = NotificationDration,
            });
        }

        public void NotifyInfo(string message, string detail = default)
        {
            notificationService.Notify(new()
            {
                Summary = stringLocalizer[message],
                Detail = detail,
                Severity = NotificationSeverity.Info,
                Duration = NotificationDration,
            });
        }

        public void NotifyError(string message, string detail = default)
        {
            notificationService.Notify(new()
            {
                Summary = stringLocalizer[message],
                Detail = detail,
                Severity = NotificationSeverity.Error,
                Duration = NotificationDration,
            });
        }

        public void NotifySuccess(string message, string detail = default)
        {
            notificationService.Notify(new()
            {
                Summary = stringLocalizer[message],
                Detail = detail,
                Severity = NotificationSeverity.Success,
                Duration = NotificationDration,
            });
        }

    }
}
