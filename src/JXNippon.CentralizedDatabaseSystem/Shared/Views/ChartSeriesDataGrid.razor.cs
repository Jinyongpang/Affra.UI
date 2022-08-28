using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ChartSeriesDataGrid
    {
        private RadzenDataGrid<ChartSeries> grid;
        private IEnumerable<ChartSeries> items;

        [Parameter] public Chart Chart { get; set; }
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
            items = Chart.ChartSeries;
        }

        public Task ReloadAsync()
        {
            return grid.FirstPage(true);
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private async Task ShowDialogAsync(ChartSeries data, int menuAction, string title)
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
                    Chart.ChartSeries.Remove(data);
                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<ChartSeriesDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction }, { "Chart", Chart }, { "Types", Types } },
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
                            Chart.ChartSeries.Add(data);
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
