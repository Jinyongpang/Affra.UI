using System.Dynamic;
using System.Linq.Dynamic.Core;
using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
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
        private readonly IHubSubscription subscription;
        private bool isDisposed = false;
        private IDictionary<string, GridColumn> gridColumnDictionary;

        [Parameter] public IDateFilterComponent DateFilter { get; set; }
        [Parameter] public EventCallback<IQueryable> LoadData { get; set; }
        [Parameter] public IQueryable Queryable { get; set; }
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

        private readonly IEnumerable<int> pageSizeOptions = new int[] { 5, 10, 15 };

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
            gridColumnDictionary = GridColumns.ToDictionary(x => $"{x.Type}{x.Property}");
            if (!string.IsNullOrEmpty(Subscription))
            {
                await ContentUpdateNotificationService.SubscribeAsync(Subscription, OnContentUpdateAsync);
            }
            if (DateFilter is not null)
            {
                DateFilter.OnDateRangeChanged += OnDateRangeChangedAsync;
            }
        }

        private async Task OnContentUpdateAsync(object obj)
        {
            await ReloadAsync();
            StateHasChanged();
        }
        public Task ReloadAsync()
        {
            return grid?.Reload() ?? Task.CompletedTask;
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            items = await GetDailyItemsAsync(TType, DateFilter?.Start, DateFilter?.End, args);
            itemsDictionary = await MergeOverridenTypeAsync(items) ?? new List<ExpandoObject>();
            isLoading = false;
            this.StateHasChanged();
        }

        private async Task<IEnumerable<IDictionary<string, object>>?> MergeOverridenTypeAsync(IEnumerable<IDaily> dailyItems)
        {
            var types = GridColumns
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
                        list.AddRange(await GetDailyItemsAsync(actualType.AssemblyQualifiedName, dailyItems.First().Date, dailyItems.Last().Date));
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

        private string GetOperator(FilterOperator filterOperator)
        {
            switch (filterOperator)
            {
                case FilterOperator.Equals: return "==";
                case FilterOperator.NotEquals: return "!=";
                case FilterOperator.GreaterThan: return ">";
                case FilterOperator.LessThan: return "<";
                case FilterOperator.GreaterThanOrEquals: return ">=";
                case FilterOperator.LessThanOrEquals: return "<=";
                case FilterOperator.Contains:
                    break;
                case FilterOperator.StartsWith:
                    break;
                case FilterOperator.EndsWith:
                    break;
                case FilterOperator.DoesNotContain:
                    break;
                case FilterOperator.IsNull:
                    break;
                case FilterOperator.IsEmpty:
                    break;
                case FilterOperator.IsNotNull:
                    break;
                case FilterOperator.IsNotEmpty:
                    break;
                default: return null;
            }
            return null;
        }

        private async Task<IEnumerable<IDaily>> GetDailyItemsAsync(string type, DateTimeOffset? start, DateTimeOffset? end, LoadDataArgs args = null)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var service = ViewService.GetGenericService(serviceScope, type);
            Queryable = service.Get();
            Queryable = Queryable
                .AppendQuery(orderBy: args?.OrderBy);

            if (args?.Filters is not null)
            {
                foreach (var filter in args?.Filters)
                {
                    if (filter.FilterValue is not null
                        && filter.FilterValue as string != string.Empty)
                    {
                        try
                        {
                            Queryable = Queryable
                                   .Where($"x => x.{filter.Property} {GetOperator(filter.FilterOperator)} \"{filter.FilterValue}\"");

                        }
                        catch (Exception ex)
                        {
                            AffraNotificationService.NotifyException(ex);
                        }
                    }
                }
            }
            
            if (start != null && end != null)
            {
                Queryable = Queryable
                    .Cast<IDaily>()
                    .Where(item => item.Date >= start.Value.ToUniversalTime())
                    .Where(item => item.Date <= end.Value.ToUniversalTime());
            }

            if (args?.Sorts is null)
            {
                Queryable = Queryable
                    .Cast<IDaily>()
                    .OrderBy(x => x.Date);
            }

            Queryable = Queryable
                .AppendQuery(null, args?.Skip, args?.Top, null);


            if (args is not null)
            {
                await LoadData.InvokeAsync(Queryable);
            }

            var q = (DataServiceQuery)Queryable;
            var response = (await q
                .ExecuteAsync()) as QueryOperationResponse;

            if (args is not null)
            {
                Count = (int)response.Count;
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
            if (gridColumnDictionary.TryGetValue(args.Column.Property, out var gridColumn))
            {
                if (args.Data.TryGetValue(args.Column.Property, out object value))
                {
                    var result = GetResultString(gridColumn, value);
                    args.Attributes.Add("style", GetStyle(gridColumn, result));
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
                    grid.Dispose();
                    if (!string.IsNullOrEmpty(Subscription))
                    {
                        await ContentUpdateNotificationService.RemoveHandlerAsync(Subscription, OnContentUpdateAsync);
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
