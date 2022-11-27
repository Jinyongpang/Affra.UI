using System.Linq.Dynamic.Core;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.AvailabilityAndReliabilityReport;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class AvailabilityAndReliabilityReportGrid
    {
        private readonly AntDesign.Menu menu;
        private readonly string search;
        private bool isLoading = false;
        private RadzenDataGrid<AvailabilityAndReliability> grid;
        private IEnumerable<AvailabilityAndReliability> items;
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        public int Count { get; set; }
        public CommonFilter Filter { get; set; }

        protected override Task OnInitializedAsync()
        {
            Filter = new CommonFilter(navigationManager);
            return base.OnInitializedAsync();
        }

        public Task ReloadAsync()
        {
            return grid.FirstPage(true);
        }

        public async Task ShowDialogAsync(AvailabilityAndReliability data, int menuAction, string title)
        {
            ContextMenuService.Close();
            dynamic? response;
            if (menuAction == 2)
            {
                response = await DialogService.OpenAsync<GenericConfirmationDialog>(title,
                           new Dictionary<string, object>() { },
                           new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

                if (response == true)
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = GetGenericService(serviceScope);
                    await service.DeleteAsync(data);

                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<AvailabilityAndReliabilityReportDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction } },
                           Constant.DialogOptions);

                if (response == true)
                {
                    try
                    {
                        using var serviceScope = ServiceProvider.CreateScope();
                        var service = GetGenericService(serviceScope);

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

        private IGenericService<AvailabilityAndReliability> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<AvailabilityAndReliability, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<AvailabilityAndReliability>? service = GetGenericService(serviceScope);
            var query = service.Get();

            if (Filter.DateRange?.Start != null)
            {
                var start = Filter.DateRange.Start.ToUniversalTime();
                var end = Filter.DateRange.End.ToUniversalTime();
                query = query
                    .Where(availabilityAndReliability => availabilityAndReliability.Date >= start)
                    .Where(availabilityAndReliability => availabilityAndReliability.Date <= end);
            }

            Microsoft.OData.Client.QueryOperationResponse<AvailabilityAndReliability>? response = await query
                .OrderBy(x => x.Date)
                .AppendQueryWithFilterDescriptor(args.Filters, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<AvailabilityAndReliability>();

            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;

            StateHasChanged();
        }
        private void OnRender(DataGridRenderEventArgs<AvailabilityAndReliability> args)
        {
            if (args.FirstRender)
            {
                args.Grid.Groups.Add(new GroupDescriptor() { Title = "Equipment Type", Property = "EquipmentType", SortOrder = SortOrder.Ascending });
                StateHasChanged();
            }
        }
    }
}
