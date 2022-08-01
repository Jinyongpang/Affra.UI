namespace JXNippon.CentralizedDatabaseSystem.Shared.Notifications
{
    public partial class PersonalMessageTab
    {
        private PersonalMessageList allPersonalMessageList;
        private PersonalMessageList unreadPersonalMessageList;
        private PersonalMessageList readPersonalMessageList;

        public int Count => this.allPersonalMessageList.Count;
        public int UnreadCount => this.unreadPersonalMessageList.Count;
        public int ReadCount => this.readPersonalMessageList.Count;

        public Task ReloadAllAsync()
        {
            return Task.WhenAll(
                this.allPersonalMessageList.ReloadAsync(),
                this.unreadPersonalMessageList.ReloadAsync(),
                this.readPersonalMessageList.ReloadAsync());
        }

        public async Task OnTabClickAsync(string key)
        {
            switch (key)
            {
                case "All":
                    await (this.allPersonalMessageList?.ReloadAsync() ?? Task.CompletedTask);
                    break;
                case "Unread":
                    await (this.unreadPersonalMessageList?.ReloadAsync() ?? Task.CompletedTask);
                    break;
                default:
                    await (this.readPersonalMessageList?.ReloadAsync() ?? Task.CompletedTask);
                    break;
            }
        }
    }
}
