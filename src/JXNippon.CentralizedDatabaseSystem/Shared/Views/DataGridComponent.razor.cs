using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class DataGridComponent : IAsyncDisposable
    {
        private RadzenDataGrid<IDaily> grid;
        private IEnumerable<IDaily> items;
        private bool isLoading = false;
        private IHubSubscription subscription;
        private bool isDisposed = false;

        [Parameter] public EventCallback<IQueryable<dynamic>> LoadData { get; set; }
        [Parameter] public IQueryable<dynamic> Queryable { get; set; }
        [Parameter] public string TType { get; set; }
        [Parameter] public string Subscription { get; set; }
        [Parameter] public DateTimeOffset? StartDate { get; set; }
        [Parameter] public DateTimeOffset? EndDate { get; set; }
        [Parameter] public IEnumerable<GridColumn> GridColumns { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private IContentUpdateNotificationService ContentUpdateNotificationService { get; set; }
        [Parameter] public Column Column { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        private IEnumerable<int> pageSizeOptions = new int[] { 5, 10, 15 };

        private Type Type
        {
            get
            {
                var type = Type.GetType(TType);
                return type;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(this.Subscription))
            {
                subscription = ContentUpdateNotificationService.Subscribe<object>(Subscription, OnContentUpdateAsync);
                await subscription.StartAsync();
            }           
            await ReloadAsync(StartDate, EndDate);
        }

        private Task OnContentUpdateAsync(object obj)
        {
            StateHasChanged();
            return this.ReloadAsync();
        }
        public Task ReloadAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            StartDate = startDate ?? StartDate;
            EndDate = endDate ?? EndDate;
            return grid?.FirstPage(true) ?? Task.CompletedTask;
        }

        private async Task LoadDataAsync(LoadDataArgs args)
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

            Queryable = (IQueryable<dynamic>)Queryable
                .Cast<IDaily>()
                .OrderBy(x => x.Date)
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy);

            await LoadData.InvokeAsync(Queryable);
            var q = (DataServiceQuery)Queryable;
            var response = (await q
                .ExecuteAsync()) as QueryOperationResponse;

            Count = (int)response.Count;

            items = response
                .Cast<IDaily>()
                .ToList();

            isLoading = false;
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private System.Reflection.PropertyInfo[]? GetProperties()
        {
            System.Reflection.PropertyInfo[]? properties = Type.GetProperties();
            return properties;
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!isDisposed)
                {
                    grid.Dispose();
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
