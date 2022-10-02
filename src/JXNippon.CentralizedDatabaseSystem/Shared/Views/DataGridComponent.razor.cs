using System.Dynamic;
using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
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
        private RadzenDataGrid<IDictionary<string, object>> grid;
        private IEnumerable<IDaily> items;
        private IEnumerable<IDictionary<string, object>> itemsDictionary;
        private bool isLoading = false;
        private IHubSubscription subscription;
        private bool isDisposed = false;
        private IDictionary<string, GridColumn> gridColumnDictionary;

        [Parameter] public IDateFilterComponent DateFilter { get; set; }
        [Parameter] public EventCallback<IQueryable<dynamic>> LoadData { get; set; }
        [Parameter] public IQueryable<dynamic> Queryable { get; set; }
        [Parameter] public string TType { get; set; }
        [Parameter] public string Subscription { get; set; }
        [Parameter] public IEnumerable<GridColumn> GridColumns { get; set; }
        [Parameter] public Column Column { get; set; }
        [Parameter] public int PageSize { get; set; }
        [Parameter] public int PageNumbersCount { get; set; }
        
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private IContentUpdateNotificationService ContentUpdateNotificationService { get; set; }
        
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
            gridColumnDictionary = this.GridColumns.ToDictionary(x => $"{x.Type}{x.Property}");
            if (!string.IsNullOrEmpty(this.Subscription))
            {
                await this.ContentUpdateNotificationService.SubscribeAsync(Subscription, OnContentUpdateAsync);
            }
            if (DateFilter is not null)
            {
                DateFilter.OnDateRangeChanged += this.OnDateRangeChangedAsync;
            }
            await ReloadAsync();
        }

        private Task OnContentUpdateAsync(object obj)
        {
            StateHasChanged();
            return this.ReloadAsync();
        }
        public Task ReloadAsync()
        {
            return grid?.FirstPage(true) ?? Task.CompletedTask;
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            this.items = await this.GetDailyItemsAsync(this.TType, DateFilter?.Start, DateFilter?.End, args);
            itemsDictionary = await this.MergeOverridenTypeAsync(this.items) ?? new List<ExpandoObject>();
            isLoading = false;
        }

        private async Task<IEnumerable<IDictionary<string, object>>?> MergeOverridenTypeAsync(IEnumerable<IDaily> dailyItems)
        {
            var types = this.GridColumns
                .Where(x => !string.IsNullOrEmpty(x.Type))
                .Select(x => x.Type)
                .Distinct();
            List<IDictionary<string, object>>? result = new List<IDictionary<string, object>>();
            List<IDaily> list = new List<IDaily>();
            if (dailyItems.Any())
            {
                foreach (var type in types)
                {
                    var actualType = ViewHelper.GetActualType(type);
                    if (actualType is not null)
                    {
                        list.AddRange(await this.GetDailyItemsAsync(actualType.AssemblyQualifiedName, dailyItems.First().Date, dailyItems.Last().Date));
                    }
                }
            }
            foreach (var item in dailyItems)
            {
                var dictionaryObject = item.ToDictionaryObject();
                foreach (var matched in list.Where(x => x.Date.ToLocalTime().Date == item.Date.ToLocalTime().Date))
                {
                    foreach (var dict in matched.ToDictionaryObject(matched.GetType().Name))
                    {
                        dictionaryObject.Add(dict);
                    }
                }
                    
                result.Add(dictionaryObject);
            }

            return result;
            
        }

        private string GetColumnPropertyExpression(string name, Type type)
        {
            var expression = $@"it[""{name}""]";
            return type == typeof(int) ? $"int.Parse({expression})" : expression;
        }

        private async Task<IEnumerable<IDaily>> GetDailyItemsAsync(string type, DateTimeOffset? start, DateTimeOffset? end, LoadDataArgs args = null)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.ViewService.GetGenericService(serviceScope, type);
            Queryable = service.Get();
            if (start != null && end != null)
            {
                Queryable = Queryable
                    .Cast<IDaily>()
                    .Where(item => item.Date >= start.Value.ToUniversalTime())
                    .Where(item => item.Date <= end.Value.ToUniversalTime());
            }

            Queryable = (IQueryable<dynamic>)Queryable
                .Cast<IDaily>()
                .OrderBy(x => x.Date)
                .AppendQuery(args?.Filter, args?.Skip, args?.Top, args?.OrderBy);

            if (args is not null)
            {
                await LoadData.InvokeAsync(Queryable);
            }
            
            var q = (DataServiceQuery)Queryable;
            var response = (await q
                .ExecuteAsync()) as QueryOperationResponse;

            if (args is not null)
            {
                this.Count = (int)response.Count;
            }

            return response
                .Cast<IDaily>()
                .ToList();
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

        private void CellRender(DataGridCellRenderEventArgs<IDictionary<string, object>> args)
        {
            if (this.gridColumnDictionary.TryGetValue(args.Column.Property, out var gridColumn))
            {
                if (args.Data.TryGetValue(args.Column.Property, out object value))
                {
                    var result = this.GetResultString(gridColumn, value);
                    args.Attributes.Add("style", this.GetStyle(gridColumn, result));
                }
            }  
        }

        private string GetResultString(GridColumn gridColumn, object input)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(gridColumn.FormatString))
            {
                result = string.Format(gridColumn.FormatString, input);
            }
            else
            {
                result = input?.ToString();
            }
            return result;
        }

        private string GetStyle(GridColumn gridColumn, string value)
        {
            if (gridColumn?.ConditionalStylings is null)
            { 
                return string.Empty;
            }

            foreach (var style in gridColumn.ConditionalStylings)
            {
                switch (style.Operator)
                {
                    case ConditionalStylingOperator.IsNotNull:
                        {
                            if (value is not null)
                            {
                                return $"{style.Style} width: 100% !important; background-color: {style.BackgroundColor}; color: {style.FontColor};";
                            }
                            break;
                        }
                    case ConditionalStylingOperator.IsNull:
                        {
                            if (value is null)
                            {
                                return $"{style.Style} background-color: {style.BackgroundColor}; color: {style.FontColor};";
                            }
                            break;
                        }
                    case ConditionalStylingOperator.Equal:
                        {
                            if (value?.Equals(style.Value, StringComparison.InvariantCultureIgnoreCase) ?? false)
                            {
                                    return $"{style.Style} background-color: {style.BackgroundColor}; color: {style.FontColor};";
                            }
                            break;
                        }
                    case ConditionalStylingOperator.NotEqual:
                        {
                            if ((!value?.Equals(style.Value, StringComparison.InvariantCultureIgnoreCase)) ?? false)
                            {
                                return $"{style.Style} background-color: {style.BackgroundColor}; color: {style.FontColor};";
                            }
                            break;
                        }
                    case ConditionalStylingOperator.Contains:
                        {
                            if (value?.Contains(style.Value, StringComparison.InvariantCultureIgnoreCase) ?? false)
                            {
                                return $"{style.Style} background-color: {style.BackgroundColor}; color: {style.FontColor};";
                            }
                            break;
                        }
                    case ConditionalStylingOperator.NotContains:
                        {
                            if ((!value?.Contains(style.Value, StringComparison.InvariantCultureIgnoreCase)) ?? false)
                            {
                                return $"{style.Style} background-color: {style.BackgroundColor}; color: {style.FontColor};";
                            }
                            break;
                        }
                    default:
                        return string.Empty;
                }
            }

            return string.Empty;
        }

        private Task OnDateRangeChangedAsync(DateRange dateRange)
        {
            return this.ReloadAsync();
        }
        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!isDisposed)
                {
                    if (DateFilter is not null)
                    {
                        DateFilter.OnDateRangeChanged -= this.OnDateRangeChangedAsync;
                    }
                    grid.Dispose();
                    if (!string.IsNullOrEmpty(this.Subscription))
                    {
                        await this.ContentUpdateNotificationService.RemoveHandlerAsync(Subscription, OnContentUpdateAsync);
                    }
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
