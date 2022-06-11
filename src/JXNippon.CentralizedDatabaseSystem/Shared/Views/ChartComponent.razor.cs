using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ChartComponent
    {
        private RadzenChart chart;
        private IEnumerable<IDaily> items;
        private bool isLoading = false;

        [Parameter] public EventCallback<IQueryable<dynamic>> LoadData { get; set; }
        [Parameter] public string FormatString { get; set; }
        [Parameter] public object Step { get; set; }
        [Parameter] public string AxisTitle { get; set; }
        [Parameter] public IEnumerable<ChartSeries> ChartSeries { get; set; }
        [Parameter] public ChartType ChartType { get; set; }
        [Parameter] public IQueryable<dynamic> Queryable { get; set; }
        [Parameter] public string TType { get; set; }
        [Parameter] public DateTimeOffset? StartDate { get; set; }
        [Parameter] public DateTimeOffset? EndDate { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        protected override Task OnInitializedAsync()
        {
            return ReloadAsync(StartDate, EndDate);
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
            var service = this.GetGenericService(serviceScope);
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

        private IEnumerable<SeriesItem> GetSeriesItems(ChartSeries chartSeries, IEnumerable<IDaily> dailyItems)
        {
            List<SeriesItem> seriesItem = new List<SeriesItem>();

            if (dailyItems != null)
            {
                foreach (var item in dailyItems)
                {
                    if ((decimal?)GetPropValue(item, chartSeries.ValueProperty) != null)
                    {
                        seriesItem.Add(new SeriesItem()
                        {
                            Category = GetPropValue(item, chartSeries.CategoryProperty),
                            Value = (decimal?)GetPropValue(item, chartSeries.ValueProperty)
                        });
                    }
                }
            }

            return seriesItem;
        }

        private object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private dynamic GetGenericService(IServiceScope serviceScope)
        {
            var type = typeof(IUnitGenericService<,>).MakeGenericType(Type.GetType(TType), typeof(ICentralizedDatabaseSystemUnitOfWork));
            return serviceScope.ServiceProvider.GetRequiredService(type);
        }
    }
}
