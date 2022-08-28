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
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ChartComponent : IAsyncDisposable
    {
        private RadzenChart chart;
        private IDictionary<string, IEnumerable<IDaily>> items;
        private bool isLoading = false;
        private List<IHubSubscription> subscriptions = new List<IHubSubscription>();
        private bool isDisposed = false;
        private HashSet<string> types = new HashSet<string>();

        [Parameter] public EventCallback<IQueryable<dynamic>> LoadData { get; set; }
        [Parameter] public string FormatString { get; set; }
        [Parameter] public object Step { get; set; }
        [Parameter] public object ValueAxisStep { get; set; }
        [Parameter] public string AxisTitle { get; set; }
        [Parameter] public IEnumerable<ChartSeries> ChartSeries { get; set; }
        [Parameter] public IQueryable<dynamic> Queryable { get; set; }
        [Parameter] public Type TType { get; set; }
        [Parameter] public bool HasSubscription { get; set; }
        [Parameter] public DateTimeOffset? StartDate { get; set; }
        [Parameter] public DateTimeOffset? EndDate { get; set; }
        [Parameter] public Column Column { get; set; }
        [Parameter] public ICollection<string> Colors { get; set; } = Array.Empty<string>();

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private IContentUpdateNotificationService ContentUpdateNotificationService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var actualTypes = new HashSet<Type>();
            actualTypes.Add(this.TType);
            types.Add(this.TType.AssemblyQualifiedName);
            foreach (var series in this.ChartSeries)
            {
                if (series.ActualType is not null
                    && !types.Contains(series.ActualType.AssemblyQualifiedName))
                {
                    types.Add(series.ActualType.AssemblyQualifiedName);
                    actualTypes.Add(series.ActualType);
                }
            }
            if (this.HasSubscription)
            {
                foreach (var type in actualTypes)
                {
                    var subscription = ContentUpdateNotificationService.Subscribe<object>(type.Name, OnContentUpdateAsync);
                    await subscription.StartAsync();
                    subscriptions.Add(subscription);
                }
            }
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
            items = new Dictionary<string, IEnumerable<IDaily>>();
            foreach (var type in this.types)
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var service = this.ViewService.GetGenericService(serviceScope, type);
                Queryable = service.Get();
                if (StartDate != null && EndDate != null)
                {
                    Queryable = Queryable
                        .Cast<IDaily>()
                        .Where(item => item.Date >= StartDate.Value.ToUniversalTime())
                        .Where(item => item.Date <= EndDate.Value.ToUniversalTime());
                }
                else
                {
                    Queryable = Queryable.Take(100);
                }
                await LoadData.InvokeAsync(Queryable);
                var q = (DataServiceQuery)Queryable;

                var typeItems = (await q.ExecuteAsync())
                    .Cast<IDaily>()
                    .OrderBy(x => x.Date)
                    .ToList();
                this.items.Add(type, typeItems);
            }

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
                    var groupItems = dailyItems.GroupBy(x =>
                        isGroup
                        ? (string)ViewService.GetPropValue(x, chartSeries.GroupProperty)
                        : chartSeries.Title);

                    foreach (var group in groupItems)
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
                        Title = AxisTitle,
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
