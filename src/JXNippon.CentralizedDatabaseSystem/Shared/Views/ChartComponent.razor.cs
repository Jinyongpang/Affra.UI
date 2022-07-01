using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ChartComponent : IAsyncDisposable
    {
        private RadzenChart chart;
        private IEnumerable<IDaily> items;
        private bool isLoading = false;
        private IHubSubscription subscription;
        private bool isDisposed = false;

        [Parameter] public string Icon { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public EventCallback<IQueryable<dynamic>> LoadData { get; set; }
        [Parameter] public string FormatString { get; set; }
        [Parameter] public object Step { get; set; }
        [Parameter] public object ValueAxisStep { get; set; }
        [Parameter] public string AxisTitle { get; set; }
        [Parameter] public IEnumerable<ChartSeries> ChartSeries { get; set; }
        [Parameter] public IQueryable<dynamic> Queryable { get; set; }
        [Parameter] public string TType { get; set; }
        [Parameter] public string Subscription { get; set; }
        [Parameter] public DateTimeOffset? StartDate { get; set; }
        [Parameter] public DateTimeOffset? EndDate { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private IContentUpdateNotificationService ContentUpdateNotificationService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        protected override async Task OnInitializedAsync()
        {
            subscription = ContentUpdateNotificationService.Subscribe<object>(Subscription, OnContentUpdateAsync);
            await subscription.StartAsync();
            await ReloadAsync(StartDate, EndDate);
            await base.OnInitializedAsync();
        }
        
        private Task OnContentUpdateAsync(object obj)
        {
            StateHasChanged();
            return this.ReloadAsync();
        }

        public async Task ReloadAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            StartDate = startDate ?? StartDate;
            EndDate = endDate ?? EndDate;
            await LoadDataAsync();
            await chart.Reload();
        }

        private async Task LoadDataAsync()
        {
            isLoading = true;

            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.ViewService.GetGenericService(serviceScope, TType);
            Queryable = service.Get();
            if (StartDate != null && EndDate != null)
            {
                Queryable = Queryable
                    .Cast<IDaily>()
                    .Where(item => item.Date >= StartDate.Value.ToUniversalTime())
                    .Where(item => item.Date <= EndDate.Value.ToUniversalTime());
            }
            await LoadData.InvokeAsync(Queryable);
            var q = (DataServiceQuery)Queryable;

            items = (await q.ExecuteAsync())
                .Cast<IDaily>()
                .OrderBy(x => x.Date)
                .ToList();

            isLoading = false;
        }

        private List<SeriesItem> GetSeriesItems(ChartSeries chartSeries, IEnumerable<IDaily> dailyItems)
        {
            List<SeriesItem> seriesItem = new List<SeriesItem>();
            bool isAPie = chartSeries.ChartType == ChartType.PieChart || chartSeries.ChartType == ChartType.DonutChart;
            if (dailyItems != null)
            {
                foreach (var item in dailyItems)
                {
                    decimal? value = null;
                    if (ViewService.GetPropValue(item, chartSeries.ValueProperty) is decimal result)
                    {
                        value = result;
                    }

                    if (value != null || isAPie)
                    {
                        seriesItem.Add(new SeriesItem()
                        {
                            Category = ViewService.GetPropValue(item, chartSeries.CategoryProperty),
                            Value = value
                        });
                    }
                }
            }

            return seriesItem;
        }

        private IEnumerable<Series> GetSeries(ChartSeries chartSeries, IEnumerable<IDaily> dailyItems)
        {
            List<Series> seriesList = new List<Series>();

            if (dailyItems != null)
            {
                bool isAPie = chartSeries.ChartType == ChartType.PieChart || chartSeries.ChartType == ChartType.DonutChart;
                if (!isAPie)
                {
                    bool isGroup = !string.IsNullOrEmpty(chartSeries.GroupProperty);
                    var groupItems = items.GroupBy(x =>
                        isGroup
                        ? (string)ViewService.GetPropValue(x, chartSeries.GroupProperty)
                        : chartSeries.Title);

                    foreach (IGrouping<string, IDaily>? group in groupItems)
                    {
                        Series series = new()
                        {
                            Title = group.Key,
                            SeriesItems = GetSeriesItems(chartSeries, group),
                        };
                        seriesList.Add(series);
                    }
                }
                else
                {
                    var groupItems = items.GroupBy(x => (string)ViewService.GetPropValue(x, chartSeries.CategoryProperty));
                    var itemsList = new List<SeriesItem>();
                    Series series = new()
                    {
                        Title = Title,
                        SeriesItems = itemsList,
                    };

                    foreach (IGrouping<string, IDaily>? group in groupItems)
                    {
                        var seriesItems = GetSeriesItems(chartSeries, group);
                        SeriesItem seriesItem = new()
                        {
                            Category = group.Key,
                            Value = GetExecutedValue(seriesItems, chartSeries.ExecutionType)
                        };

                        itemsList.Add(seriesItem);
                    }
                    seriesList.Add(series);
                }
            }

            return seriesList;
        }

        private decimal GetExecutedValue(List<SeriesItem> seriesItems, ExecutionType executionType)
        {
            return executionType switch
            {
                ExecutionType.Average => (decimal)seriesItems.Average(x => x.Value),
                ExecutionType.Distinct => seriesItems.DistinctBy(x => x.Value).Count(),
                ExecutionType.Sum => (decimal)seriesItems.Sum(x => x.Value),
                ExecutionType.Max => (decimal)seriesItems.Max(x => x.Value),
                ExecutionType.Min => (decimal)seriesItems.Min(x => x.Value),
                _ => seriesItems.Count(),
            };
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!isDisposed)
                {
                    chart.Dispose();
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
