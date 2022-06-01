using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class Chart
    {
        private RadzenChart chart;
        private IEnumerable<IDaily> items;
        private bool isLoading = false;

        [Parameter] public EventCallback<IQueryable<dynamic>> LoadData { get; set; }
        [Parameter] public string FormatString { get; set; }
        [Parameter] public object Step { get; set; }
        [Parameter] public string AxisTitle { get; set; }
        [Parameter] public IEnumerable<ChartSeries> ChartSeries { get; set; }
        [Parameter] public IQueryable<dynamic> Queryable { get; set; }
        [Parameter] public string TType { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        protected override Task OnInitializedAsync()
        {
            return ReloadAsync();
        }

        public async Task ReloadAsync()
        {
            await LoadDataAsync();
            await chart.Reload();
        }

        private async Task LoadDataAsync()
        {
            isLoading = true;

            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScope);
            Queryable = service.Get();
            await LoadData.InvokeAsync(Queryable);
            var q = (DataServiceQuery)Queryable;
            items = (await q.ExecuteAsync())
                .Cast<IDaily>()
                .ToList();

            isLoading = false;
        }

        private IEnumerable<SeriesItem> GetSeriesItems(ChartSeries chartSeries)
        {
            List<SeriesItem> seriesItem = new List<SeriesItem>();

            if (items != null)
            {
                foreach (var item in items)
                {
                    seriesItem.Add(new SeriesItem()
                    {
                        Category = GetPropValue(item, chartSeries.CategoryProperty),
                        Value = (decimal)GetPropValue(item, chartSeries.ValueProperty)
                    });
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
