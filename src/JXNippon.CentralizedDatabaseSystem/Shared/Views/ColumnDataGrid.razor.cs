using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ColumnDataGrid
    {
        private RadzenDataGrid<Column> grid;
        private IEnumerable<Column> items;
        private bool isLoading = false;

        [Parameter] public View View { get; set; }
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        public Task ReloadAsync()
        {
            return grid.FirstPage(true);
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            if (View is null)
            {
                return;
            }
            isLoading = true;
            await LoadData.InvokeAsync();
            using var serviceScope = ServiceProvider.CreateScope();
            var viewService = serviceScope.ServiceProvider.GetRequiredService<IViewService>();
            items = await viewService.GetColumnsAsync(View);
            items = items
                .OrderBy(x => x.RowSequence)
                .ThenBy(x => x.Sequence);
            Count = items.Count();
            isLoading = false;
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<T> GetGenericService<T>(IServiceScope serviceScope)
            where T : class
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<T, IViewUnitOfWork>>();
        }

        private async Task ShowDialogAsync(Column data, int menuAction, string title)
        {
            ContextMenuService.Close();
            dynamic? response = null;
            if (menuAction == 2)
            {
                response = await DialogService.OpenAsync<GenericConfirmationDialog>(title,
                           new Dictionary<string, object>() { },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

                if (response == true)
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService<Column>(serviceScope);
                    await service.DeleteAsync(data);

                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                if (data.ComponentType == nameof(Chart))
                {
                    response = await DialogService.OpenAsync<ChartDialog>(title,
                       new Dictionary<string, object>() { { "Column", data }, { "MenuAction", menuAction }, { "View", View } },
                       Constant.DialogOptions);
                }
                else if (data.ComponentType == nameof(Grid))
                {
                    response = await DialogService.OpenAsync<GridDialog>(title,
                       new Dictionary<string, object>() { { "Column", data }, { "MenuAction", menuAction }, { "View", View } },
                       Constant.DialogOptions);
                }


                if (response == true)
                {
                    try
                    {
                        using var serviceScope = ServiceProvider.CreateScope();
                        var service = this.GetGenericService<Column>(serviceScope);

                        if (data.Id > 0)
                        {
                            isLoading = true;
                            await service.UpdateAsync(data, data.Id);
                            AffraNotificationService.NotifyItemUpdated();
                        }
                        else
                        {
                            isLoading = true;
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
                        isLoading = false;
                    }
                }
            }

            await grid.Reload();
        }
    }
}
