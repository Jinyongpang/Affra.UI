namespace NotficationODataService.Affra.Service.Notification.Domain.PersonalMessages
{
    public partial class PersonalMessage
    {
        public string MessageContent => this.Message.Content;

        public string Subject => this.Message.Subject;

        public string Extra => this.Message.Extra;

        public string CreatedBy => this.Message.CreatedBy;
    }
}
