using System.Linq.Dynamic.Core;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class OilDefermentDetailGrid
    {
        private readonly AntDesign.Menu menu;
        private readonly string search;
        private bool isLoading = false;
        public string OilDefermentDetailFilter { get; set; }
        private RadzenDataGrid<OilDefermentDetail> grid;
        private IEnumerable<OilDefermentDetail> items;
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        public int Count { get; set; }

        public Task ReloadAsync()
        {
            return grid.FirstPage(true);
        }

        public async Task ShowDialogAsync(OilDefermentDetail data, int menuAction, string title)
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
                    var service = GetGenericService(serviceScope);
                    await service.DeleteAsync(data);

                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<OilDefermentDetailDialog>(title,
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

        private IGenericService<OilDefermentDetail> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<OilDefermentDetail, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<OilDefermentDetail>? service = GetGenericService(serviceScope);
            var query = service.Get();

            if (OilDefermentDetailFilter == "Open")
            {
                query = query.Where(x => x.Status == DefermentDetailStatus.Open);
            }
            else if (OilDefermentDetailFilter == "Closed")
            {
                query = query.Where(x => x.Status == DefermentDetailStatus.Closed);
            }

            Microsoft.OData.Client.QueryOperationResponse<OilDefermentDetail>? response = await query
                .OrderBy(x => x.StartDate)
                .AppendQueryWithFilterDescriptor(args.Filters, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<OilDefermentDetail>();

            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;

            StateHasChanged();
        }
    }
}
