using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.Announcements;
using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class AnnouncementCardComponent : IAsyncDisposable
    {
        [Parameter] public Column Column { get; set; }
        [Parameter] public AnnouncementCard AnnouncementCard { get; set; }
        [Parameter] public bool HasSubscription { get; set; }
        [Inject] private IContentUpdateNotificationService ContentUpdateNotificationService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        private List<IHubSubscription> subscriptions = new List<IHubSubscription>();
        private bool isDisposed = false;

        public Task ReloadAsync()
        {
            this.StateHasChanged();
            return Task.CompletedTask;
        }

        protected override async Task OnInitializedAsync()
        {
            if (this.HasSubscription)
            {
                var subscription = this.ContentUpdateNotificationService.Subscribe<Column>(nameof(Column), OnContentUpdateAsync);
                await subscription.StartAsync();
            }
        }

        private Task OnContentUpdateAsync(Column column)
        {
            if (this.Column?.Id == column?.Id)
            {
                this.AnnouncementCard = JsonSerializer.Deserialize<AnnouncementCard>(column.ColumnComponent);
                AffraNotificationService.NotifyInfo("Announcement updated.");
                this.StateHasChanged();
            }

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!isDisposed)
                {
                    foreach (var subscription in this.subscriptions)
                    {
                        if (subscription is not null)
                        {
                            await subscription.DisposeAsync();
                        }
                    }
                    isDisposed = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
