using System.Diagnostics.CodeAnalysis;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class GenericChart<TItem>
        where TItem : class
    {
        private RadzenChart chart;
        private IEnumerable<TItem> items;
        private bool isLoading = false;

        [Parameter] public EventCallback<IQueryable<TItem>> LoadData { get; set; }
        [Parameter] public string FormatString { get; set; }
        [Parameter] public object Step { get; set; }
        [Parameter] public string AxisTitle { get; set; }
        [Parameter] public IEnumerable<ChartSeries> ChartSeries { get; set; }
        [Parameter] public IQueryable<TItem> Queryable { get; set; }

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
            var response = await Queryable
                .ToQueryOperationResponseAsync<TItem>();

            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<TItem> GetGenericService(IServiceScope serviceScope)
        {
            var strings = typeof(IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>).FullName;
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
        }
    }
}
