using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Logistics;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class LogisticDataGrid
    {
        private RadzenDataGrid<DailyLogistic> grid;
        private IEnumerable<DailyLogistic> items;
        private bool isLoading = false;

        private IList<DailyLogistic> selectedItem;

        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        public async Task ReloadAsync()
        {
            await grid.FirstPage(true);
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            await LoadData.InvokeAsync();
            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScope);
            var query = service.Get();
            if (CommonFilter != null)
            {
                if (!string.IsNullOrEmpty(CommonFilter.Search))
                {
                    query = query.Where(x => x.LogisticName.ToUpper().Contains(CommonFilter.Search.ToUpper()));
                }

                if (CommonFilter.Date != null)
                {
                    var start = TimeZoneInfo.ConvertTimeToUtc(CommonFilter.Date.Value);
                    var end = TimeZoneInfo.ConvertTimeToUtc(CommonFilter.Date.Value.AddDays(1));
                    query = query
                        .Where(x => x.Date >= start)
                        .Where(x => x.Date < end);
                }
            }

            var response = await query
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DailyLogistic>();

            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;
        }

        private void HandleException(Exception ex)
        {
            NotificationService.Notify(new()
            {
                Summary = "Error",
                Detail = ex.InnerException?.ToString(),
                Severity = NotificationSeverity.Error,
                Duration = 120000,
            });
        }

        private IGenericService<DailyLogistic> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyLogistic, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private void OnCellContextMenu(DataGridCellMouseEventArgs<DailyLogistic> args)
        {
            ContextMenuService.Open(args,
                new List<ContextMenuItem> {
                new ContextMenuItem(){ Text = "Add", Value = 1 },
                new ContextMenuItem(){ Text = "Edit", Value = 2 },
                new ContextMenuItem(){ Text = "Detail", Value = 3 },
                new ContextMenuItem(){ Text = "Delete", Value = 4 }
                },
                async (e) => {
                    if (e.Text == "Add")
                    {
                        await ShowDialog(new DailyLogistic(), e.Text, "Add new logistic item");
                    }
                    else if (e.Text == "Edit")
                    {
                        await ShowDialog(args.Data, e.Text, "Edit logistic item");
                    }
                    else if (e.Text == "Delete")
                    {
                        await ShowDialog(args.Data, e.Text, "Delete logistic item");
                    }
                    else if (e.Text == "Detail")
                    {
                        await ShowDialog(args.Data, e.Text, "Logistic item details");
                    }
                }
             );
        }

        private async Task ShowDialog(DailyLogistic data, string menuAction, string title)
        {
            ContextMenuService.Close();

            if(menuAction == "Delete")
            {
                var response = await DialogService.OpenAsync<GenericDeleteDialog>(title,
                           new Dictionary<string, object>() { },
                           new DialogOptions() { Width = "600px", Height = "400px", Resizable = true, Draggable = true });

                if (response != null && response)
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService(serviceScope);
                    await service.DeleteAsync(data);

                    NotificationService.Notify(new()
                    {
                        Summary = "Delete successful.",
                        Detail = "",
                        Severity = NotificationSeverity.Success,
                        Duration = 120000,
                    });
                }
            }
            else
            {
                await DialogService.OpenAsync<LogisticDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction } },
                           new DialogOptions() { Width = "700px", Height = "570px", Resizable = true, Draggable = true });
            }


            await grid.Reload();
        }
    }
}
