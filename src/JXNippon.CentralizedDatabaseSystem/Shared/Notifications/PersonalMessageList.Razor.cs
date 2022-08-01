using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Notifications;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using NotficationODataService.Affra.Service.Notification.Domain.PersonalMessages;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Notifications
{
    public partial class PersonalMessageList
    {
        private const int loadSize = 9;
        private AntList<PersonalMessage> _dataList;
        private List<PersonalMessage > personalMessages;
        private int count;
        private int currentCount;
        private bool isLoading = false;
        private bool initLoading = true;

        [Parameter] public PersonalMessageStatus? PersonalMessageStatus { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        public int Count => this.count;

        protected override Task OnInitializedAsync()
        {
            initLoading = false;
            return this.LoadDataAsync();
        }

        public Task ReloadAsync()
        {
            return this.LoadDataAsync();
        }

        public Task OnLoadMoreAsync()
        {
            return this.LoadDataAsync(true);
        }

        private async Task LoadDataAsync(bool isLoadMore = false)
        {
            isLoading = true;
            StateHasChanged();
            if (!isLoadMore)
            {
                currentCount = 0;
            }

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<PersonalMessage>? personalMessageService = this.GetGenericService(serviceScope);
            var query = (DataServiceQuery<PersonalMessage>)personalMessageService.Get();

            if (this.PersonalMessageStatus is not null)
            { 
                query = (DataServiceQuery<PersonalMessage>)query.Where(x => x.Status == this.PersonalMessageStatus);
            }

            QueryOperationResponse<PersonalMessage>? response = await query
                .Expand(x => x.Message)
                .OrderByDescending(x => x.CreatedDateTime)
                .Skip(currentCount)
                .Take(loadSize)
                .ToQueryOperationResponseAsync<PersonalMessage>();

            count = (int)response.Count;
            currentCount += loadSize;
            var personalMessageList = response.ToList();

            if (isLoadMore)
            {
                personalMessages.AddRange(personalMessageList);
            }
            else
            {
                personalMessages = personalMessageList;
            }

            isLoading = false;

            if (personalMessages.DistinctBy(x => x.Id).Count() != personalMessages.Count)
            {
                AffraNotificationService.NotifyWarning("Data have changed. Kindly reload.");
            }

            StateHasChanged();
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<PersonalMessage> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<PersonalMessage, INotificationUnitOfWork>>();
        }

    }
}
