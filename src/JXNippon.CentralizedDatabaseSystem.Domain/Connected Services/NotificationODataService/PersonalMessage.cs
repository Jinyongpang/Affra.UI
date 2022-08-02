using Microsoft.OData.Client;

namespace NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages
{
    public partial class PersonalMessage
    {
        [IgnoreClientProperty]
        public string MessageContent => this.Message.Content;

        [IgnoreClientProperty]
        public string Subject => this.Message.Subject;

        [IgnoreClientProperty]
        public string Extra => this.Message.Extra;

        [IgnoreClientProperty]
        public string CreatedBy => this.Message.CreatedBy;
    }
}
