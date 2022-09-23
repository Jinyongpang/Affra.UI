using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class GridColumnDataGrid
    {
        private RadzenDataGrid<GridColumn> grid;
        private IEnumerable<GridColumn> items;

        [Parameter] public Grid Grid { get; set; }
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public IEnumerable<string> Types { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        protected override void OnInitialized()
        {
            items = Grid.GridColumns;
        }

        public Task ReloadAsync()
        {
            return grid.FirstPage(true);
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private async Task ShowDialogAsync(GridColumn data, int menuAction, string title)
        {
            ContextMenuService.Close();
            dynamic? response;
            if (menuAction == 2)
            {
                response = await DialogService.OpenAsync<GenericConfirmationDialog>(title,
                           new Dictionary<string, object>() { },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

                if (response == true)
                {
                    Grid.GridColumns.Remove(data);
                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else if (menuAction == 4)
            {
                response = await DialogService.OpenAsync<GridColumnMultiSelectDialog>(title,
                           new Dictionary<string, object>() { { "Item", Grid }, { "MenuAction", menuAction }, { "Types", Types } },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

                if (response is IEnumerable<GridColumn> gridColumns)
                {
                    try
                    {
                        foreach (var column in gridColumns)
                        {
                            Grid.GridColumns.Add(column);
                        }

                        AffraNotificationService.NotifyItemCreated();
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
            else
            {
                response = await DialogService.OpenAsync<GridColumnDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction }, { "Grid", Grid }, { "Types", Types } },
                           Constant.DialogOptions);

                if (response == true)
                {
                    try
                    {
                        if (menuAction != 0)
                        {
                            AffraNotificationService.NotifyItemUpdated();
                        }
                        else
                        {
                            Grid.GridColumns.Add(data);
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

            await grid.Reload();
        }
    }
}
