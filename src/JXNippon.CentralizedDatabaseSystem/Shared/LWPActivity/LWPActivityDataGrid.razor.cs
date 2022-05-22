using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.LWPActivities;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.LWPActivity
{
    public partial class LWPActivityDataGrid
    {
        private RadzenDataGrid<DailyLWPActivity> grid;
        private IEnumerable<DailyLWPActivity> items;
        private bool isLoading = false;

        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
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
            isLoading = true;
            await LoadData.InvokeAsync();
            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScope);
            var query = service.Get();
            if (CommonFilter != null)
            {
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
                .ToQueryOperationResponseAsync<DailyLWPActivity>();

            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<DailyLWPActivity> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyLWPActivity, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task ShowDialogAsync(DailyLWPActivity data, int menuAction, string title)
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
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService(serviceScope);
                    await service.DeleteAsync(data);

                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<LWPActivityDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction } },
                           Constant.DialogOptions);

                if (response == true)
                {
                    try
                    {
                        using var serviceScope = ServiceProvider.CreateScope();
                        var service = this.GetGenericService(serviceScope);
                        data.Date = data.Date.ToUniversalTime();

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
