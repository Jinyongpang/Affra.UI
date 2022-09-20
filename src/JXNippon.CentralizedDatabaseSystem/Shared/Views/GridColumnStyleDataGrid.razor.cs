using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class GridColumnStyleDataGrid
    {
        private RadzenDataGrid<ConditionalStyling> grid;
        private IEnumerable<ConditionalStyling> items;

        [Parameter] public GridColumn GridColumn { get; set; }
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        protected override void OnInitialized()
        {
            GridColumn.ConditionalStylings ??= new List<ConditionalStyling>();
            items = GridColumn.ConditionalStylings;
        }

        public Task ReloadAsync()
        {
            return grid.FirstPage(true);
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private async Task ShowDialogAsync(ConditionalStyling data, int menuAction, string title)
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
                    GridColumn.ConditionalStylings.Remove(data);
                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<GridColumnStyleDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction }, },
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
                            GridColumn.ConditionalStylings.Add(data);
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
