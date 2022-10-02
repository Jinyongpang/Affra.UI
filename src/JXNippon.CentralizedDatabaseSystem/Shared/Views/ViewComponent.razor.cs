using System.Collections.Concurrent;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.Announcements;
using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewComponent : IAsyncDisposable
    {
        private bool _disposed = false;

        private const string ViewUpdated = "View updated.";
        private const string DisplayNoneStyle = "display: none !important;";
        private Column draggingItem;

        private int enteredId = -1;

        private int draggedId = -1;

        private int focusId = -1;

        private bool isFirstRowCompleted = false;

        private readonly IDictionary<long, Column> columnDictionary = new ConcurrentDictionary<long, Column>();

        private readonly IDictionary<long, string> cardClasses = new ConcurrentDictionary<long, string>();

        private readonly List<ChartComponent> chartComponents = new();

        [Parameter]
        public bool HasFocus { get; set; }

        [Parameter]
        public EventCallback<bool> HasFocusChanged { get; set; }

        [Parameter] public View View { get; set; }

        [Parameter] public bool IsDesignMode { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        public ChartComponent chartComponentRef
        {
            set
            {
                var existingComponent = chartComponents
                    .Where(x => x.Column.Id == value.Column.Id)
                    .FirstOrDefault();

                if (existingComponent is not null)
                {
                    chartComponents.Remove(existingComponent);
                }

                chartComponents.Add(value);
            }
        }

        private readonly List<DataGridComponent> gridComponents = new();
        public DataGridComponent gridComponentRef
        {
            set
            {
                var existingComponent = gridComponents
                    .Where(x => x.Column.Id == value.Column.Id)
                    .FirstOrDefault();

                if (existingComponent is not null)
                {
                    gridComponents.Remove(existingComponent);
                }

                gridComponents.Add(value);
            }
        }

        private readonly List<AnnouncementCardComponent> announcementComponents = new();
        public AnnouncementCardComponent announcementComponentRef
        {
            set
            {
                var existingComponent = announcementComponents
                    .Where(x => x.Column.Id == value.Column.Id)
                    .FirstOrDefault();

                if (existingComponent is not null)
                {
                    announcementComponents.Remove(existingComponent);
                }

                announcementComponents.Add(value);
            }
        }

        private static readonly IDictionary<string, DateFilterComponent> dateFilterComponents = new ConcurrentDictionary<string, DateFilterComponent>();
        public DateFilterComponent dateFilterComponentRef
        {
            set
            {
                if (dateFilterComponents.TryGetValue(value.DateFilter.Id, out var existingComponent))
                {
                    dateFilterComponents.Remove(value.DateFilter.Id);
                }

                dateFilterComponents.Add(value.DateFilter.Id, value);
            }
        }

        private readonly List<StatisticsComponent> statisticsComponents = new();
        public StatisticsComponent statisticsComponentRef
        {
            set
            {
                var existingComponent = statisticsComponents
                    .Where(x => x.Column.Id == value.Column.Id)
                    .FirstOrDefault();

                if (existingComponent is not null)
                {
                    statisticsComponents.Remove(existingComponent);
                }

                statisticsComponents.Add(value);
            }
        }

        public async Task ReloadAsync(bool forceReload = true)
        {
            StateHasChanged();
            if (forceReload)
            {
                var allColumns = this.View.Rows.SelectMany(x => x.Columns);
                List<Task> tasks = new List<Task>();
                tasks.AddRange(chartComponents
                    .Where(x => allColumns.Any(column => column.Id == x.Column.Id))
                    .Select(x => x.ReloadAsync())
                    .ToList());
                tasks.AddRange(gridComponents
                    .Where(x => allColumns.Any(column => column.Id == x.Column.Id))
                    .Select(x => x.ReloadAsync())
                    .ToList());

                tasks.AddRange(announcementComponents
                    .Where(x => allColumns.Any(column => column.Id == x.Column.Id))
                    .Select(x => x.ReloadAsync())
                    .ToList());
                await Task.WhenAll(tasks);
            }        
        }

        private Task OnFocusAsync(int i)
        {
            this.focusId = i;
            this.HasFocus = i != -1;
            return HasFocusChanged.InvokeAsync(i != -1);
        }


        private async Task OnEditAsync(MouseEventArgs args, Column column)
        {
            await ShowDialogAsync(column);
        }

        private IGenericService<T> GetGenericService<T>(IServiceScope serviceScope)
            where T : class
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<T, IViewUnitOfWork>>();
        }

        private async Task ShowDeleteDialogAsync(MouseEventArgs args, object data)
        {
            dynamic? response = await DialogService.OpenAsync<GenericConfirmationDialog>("Delete",
                new Dictionary<string, object>() { },
                new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

            if (response == true)
            {
                using var serviceScope = ServiceProvider.CreateScope();

                if (data is Column column)
                {
                    var service = this.GetGenericService<Column>(serviceScope);
                    await service.DeleteAsync(column);
                    column.Row.Columns.Remove(column);
                }
                else if (data is Row row)
                {
                    var service = this.GetGenericService<Row>(serviceScope);
                    await service.DeleteAsync(row);
                    View.Rows.Remove(row);
                }

                StateHasChanged();
                AffraNotificationService.NotifyItemDeleted();
            }
        }

        private async Task ShowDialogAsync(Column data)
        {
            dynamic? response = null;
            if (data.ComponentType == nameof(Chart))
            {
                response = await DialogService.OpenAsync<ChartDialog>("Edit",
                    new Dictionary<string, object>() { { "Column", data }, { "View", View } },
                    Constant.FullScreenDialogOptions);
            }
            else if (data.ComponentType == nameof(Grid))
            {
                response = await DialogService.OpenAsync<GridDialog>("Edit",
                    new Dictionary<string, object>() { { "Column", data }, { "View", View } },
                    Constant.FullScreenDialogOptions);
            }
            else if (data.ComponentType == nameof(AnnouncementCard))
            {
                response = await DialogService.OpenAsync<AnnouncementCardDialog>("Edit",
                    new Dictionary<string, object>() { { "Column", data }, { "View", View } },
                    Constant.FullScreenDialogOptions);
            }
            else if (data.ComponentType == nameof(DateFilter))
            {
                response = await DialogService.OpenAsync<DateFilterDialog>("Edit",
                    new Dictionary<string, object>() { { "Column", data }, { "View", View } },
                    Constant.FullScreenDialogOptions);
            }
            else if (data.ComponentType == nameof(Domain.Statistics.Statistic))
            {
                response = await DialogService.OpenAsync<StatisticsDialog>("Edit",
                    new Dictionary<string, object>() { { "Column", data }, { "View", View } },
                    Constant.FullScreenDialogOptions);
            }

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService<Column>(serviceScope);

                    if (data.Id > 0)
                    {
                        await service.UpdateAsync(data, data.Id);
                        AffraNotificationService.NotifyItemUpdated();
                    }
                    else
                    {
                        await service.InsertAsync(data);
                        AffraNotificationService.NotifyItemCreated();
                    }

                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
                finally
                {
                }
            }
        }

        private string draggableClass
        {
            get
            {
                return IsDesignMode
                    ? "draggable"
                    : string.Empty;
            }
        }
        private string isDraggable
        {
            get
            {
                return IsDesignMode
                    ? "true"
                    : "false";
            }
        }

        private void HandleDragEnter(int i)
        {
            if (!IsDesignMode)
            {
                return;
            }
            cardClasses[i] = "can-drop";
            enteredId = i;
        }

        private void HandleDragLeave(int i)
        {
            if (!IsDesignMode)
            {
                return;
            }
            cardClasses[i] = string.Empty;
            enteredId = -1;
        }

        private string GetFocusClass(int? i, string widthGridSize)
        {
            if (this.focusId == -1)
            {
                return widthGridSize;
            }
            else if (this.CheckIsFocused(i))
            {
                return "focused-card col";
            }
            else
            {
                return "hidden-card";
            }
        }

        private string GetCardFocusClass(int? i)
        {
            return this.CheckIsFocused(i)
                ? "focused"
                : string.Empty;
        }

        private int GetPageSize(int? i)
        {
            return this.CheckIsFocused(i)
                ? 10
                : 5;
        }

        private int GetPageNumbersCount(int? i)
        {
            return this.CheckIsFocused(i)
                ? 5
                : 1;
        }

        private bool CheckIsFocused(int? i)
        {
            return this.focusId == i;
        }

        private void HandleDragStart(DragEventArgs arg, Row row, Column column, int draggedId)
        {
            arg.DataTransfer.DropEffect = "move";
            draggingItem = columnDictionary[column.Id];
            this.draggedId = draggedId;
        }

        private async Task HandleDropOnColumnAsync(DragEventArgs arg, Column column)
        {
            if (!IsDesignMode || column.Id == draggingItem?.Id || column is null || draggingItem is null)
            {
                return;
            }
            try
            {
                this.UpdateColumn(this.columnDictionary[draggingItem.Id], draggingItem);
                draggingItem.RowId = column.RowId;
                draggingItem.RowSequence = column.RowSequence;
                draggingItem.Sequence = column.Sequence;
                using var serviceScope = ServiceProvider.CreateScope();
                await this.GetGenericService<Column>(serviceScope).UpdateAsync(draggingItem, draggingItem.Id);
                await this.RefreshViewAsync();

                AffraNotificationService.NotifyInfo(ViewUpdated);
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }

        private async Task HandleDropOnRowAsync(DragEventArgs arg, Row row)
        {
            if (!IsDesignMode || row is null || draggingItem is null)
            {
                return;
            }

            try
            {
                this.UpdateColumn(this.columnDictionary[draggingItem.Id], draggingItem);
                draggingItem.RowId = row.Id;
                draggingItem.RowSequence = row.Sequence;
                draggingItem.Sequence = 0;
                row.Columns.Add(draggingItem);
                using var serviceScope = ServiceProvider.CreateScope();
                await this.GetGenericService<Column>(serviceScope).UpdateAsync(draggingItem, draggingItem.Id);
                await RefreshViewAsync();

                AffraNotificationService.NotifyInfo(ViewUpdated);
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }

        private async Task RefreshViewAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var viewService = serviceScope.ServiceProvider.GetRequiredService<IViewService>();
            await viewService.GetViewDetailAsync(View);
            foreach (var row in View.Rows)
            {
                foreach (var column in row.Columns)
                {
                    UpdateColumnDictionary(column);
                }
            }
            await this.ReloadAsync(false);
        }

        private void UpdateColumnDictionary(Column col)
        {
            if (this.columnDictionary.TryGetValue(col.Id, out var value))
            {
                this.UpdateColumn(col, value);
            }
            else
            {
                this.columnDictionary.Add(col.Id, col);
            }
        }

        private void UpdateColumn(Column source, Column target)
        {
            if (target.xmin != source.xmin)
            {
                target.Sequence = source.Sequence;
                target.RowSequence = source.RowSequence;
                target.RowId = source.RowId;
                target.xmin = source.xmin;
                target.Row = source.Row;
            }
        }

        private IDateFilterComponent GetDateFilterComponent(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            else if (id == Constant.GlobalDateFilter)
            {
                return GlobalDataSource.GlobalDateFilter;
            }
            else if (dateFilterComponents.TryGetValue(id, out var component))
            {
                return component.Filter;
            }
            else
            {
                return null;
            }
        }

        private string CheckOnDrag(Column col)
        {
            return this.enteredId == (int)col.Id && this.draggedId != (int)col.Id && (col.RowId != draggingItem.RowId || col.Sequence != draggingItem.Sequence + 1)
                ? string.Empty
                : DisplayNoneStyle;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                foreach (var component in chartComponents)
                {
                    await component.DisposeAsync();
                }

                foreach (var component in gridComponents)
                {
                    await component.DisposeAsync();
                }
                chartComponents.Clear();
                gridComponents.Clear();
                _disposed = true;
            }
        }
    }
}
