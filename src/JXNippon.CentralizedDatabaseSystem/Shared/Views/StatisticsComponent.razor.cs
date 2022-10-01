using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Domain.Statistics;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class StatisticsComponent : IAsyncDisposable
    {
        [Parameter] public Column Column { get; set; }
        [Parameter] public Statistic Statistic { get; set; }
        [Parameter] public string TType { get; set; }
        [Parameter] public string Subscription { get; set; }
        [Parameter] public IDateFilterComponent DateFilter { get; set; }
        [Inject] private IContentUpdateNotificationService ContentUpdateNotificationService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IViewService ViewService { get; set; }

        private readonly List<IHubSubscription> subscriptions = new List<IHubSubscription>();
        private bool isDisposed = false;
        private object? value;
        private object? previousValue;
        private bool isLoading = false;
        public async Task ReloadAsync()
        {
            await this.LoadDataAsync();
            StateHasChanged();
        }
        private async Task LoadDataAsync()
        {
            isLoading = true;
            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.ViewService.GetGenericService(serviceScope, TType);
            IQueryable<dynamic> queryable = service.Get();

            if (DateFilter?.Start != null && DateFilter?.End != null)
            {
                DateTime start = DateFilter.Start.Value.ToUniversalTime();
                if (this.Statistic.ComparePrevious)
                {
                    start = start.AddDays(-1);

                    if (TType.StartsWith("Monthly"))
                    {
                        start = start.AddMonths(-1);
                    }
                }
                queryable = queryable
                    .Cast<IDaily>()
                    .Where(item => item.Date >= start)
                    .Where(item => item.Date <= DateFilter.End.Value.ToUniversalTime());
            }

            var q = (DataServiceQuery<IDaily>)queryable
                .Cast<IDaily>()
                .OrderByDescending(x => x.Date)
                .Take(2);


            var typeItems = (await q.ExecuteAsync())
                .ToList();

            value = null;
            previousValue = null;

            if (typeItems.Any())
            {
                value = ViewService.GetPropValue(typeItems.FirstOrDefault(), this.Statistic.Property);
            }

            if (typeItems.Count > 1)
            {
                previousValue = ViewService.GetPropValue(typeItems[1], this.Statistic.Property);
            }

            isLoading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Subscription))
            {
                var subscription = ContentUpdateNotificationService.Subscribe<object>(Subscription, OnContentUpdateAsync);
                await subscription.StartAsync();
            }

            if (DateFilter is not null)
            {
                DateFilter.OnDateRangeChanged += OnDateRangeChangedAsync;
            }

            await this.ReloadAsync();
        }

        private Task OnDateRangeChangedAsync(DateRange dateRange)
        {
            return ReloadAsync();
        }

        private Task OnContentUpdateAsync(object update)
        {

            return ReloadAsync();
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!isDisposed)
                {
                    if (DateFilter is not null)
                    {
                        DateFilter.OnDateRangeChanged -= OnDateRangeChangedAsync;
                    }
                    foreach (var subscription in subscriptions)
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
