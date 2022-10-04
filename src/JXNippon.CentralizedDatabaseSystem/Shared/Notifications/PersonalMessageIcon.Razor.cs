using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using JXNippon.CentralizedDatabaseSystem.Domain.Notifications;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Notifications
{
    public partial class PersonalMessageIcon : IAsyncDisposable
    {
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IPersonalMessageNotificationService PersonalMessageNotificationService { get; set; }
        [Inject] private AntDesign.DrawerService DrawerService { get; set; }

        private IHubSubscription subscription;
        private bool isDisposed = false;
        private PersonalMessageTab personalMessageTab;
        private bool visible = false;
        private AntDesign.Drawer drawer;
        protected override async Task OnInitializedAsync()
        {
            subscription = PersonalMessageNotificationService.Subscribe(OnMessageReceivedAsync);

            try
            {
                await subscription.StartAsync();
                await this.GetUnreadCountAsync();
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.ToString());
                GlobalDataSource.AddException(ex);    
            }
        }

        private async Task OnMessageReceivedAsync(Message obj)
        {
            AffraNotificationService.NotifyInfo(obj.Content, obj.Subject);

            await Task.WhenAll(this.GetUnreadCountAsync(), this.personalMessageTab.ReloadAllAsync());
        }

        private void Open()
        {
            this.visible = true;
        }

        private Task CloseAsync()
        {
            this.visible = false;
            return this.GetUnreadCountAsync();
        }

        private IGenericService<PersonalMessage> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<PersonalMessage, INotificationUnitOfWork>>();
        }

        private async Task GetUnreadCountAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<PersonalMessage>? personalMessageService = this.GetGenericService(serviceScope);
            var query = (DataServiceQuery<PersonalMessage>)personalMessageService.Get();

            QueryOperationResponse<PersonalMessage>? response = await query
                .Where(x => x.Status == PersonalMessageStatus.Unread)
                .OrderByDescending(x => x.CreatedDateTime)
                .Take(0)
                .ToQueryOperationResponseAsync<PersonalMessage>();

            GlobalDataSource.UnreadCount = (int)response.Count;
            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!isDisposed)
                {
                    if (subscription is not null)
                    {
                        await subscription.DisposeAsync();
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
