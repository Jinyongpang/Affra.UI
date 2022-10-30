using System.Linq.Dynamic.Core;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.AuditTrails;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;
using Radzen.Blazor;
using WellProductionCalculations = CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations;

namespace JXNippon.CentralizedDatabaseSystem.Shared.WellCoefficient
{
    public partial class WellCoefficientGrid
    {
        private readonly AntDesign.Menu menu;
        private readonly string search;
        private bool isLoading = false;
        public string DefermentDetailFilter { get; set; }
        private RadzenDataGrid<WellProductionCalculations.WellCoefficient> grid;
        private IEnumerable<WellProductionCalculations.WellCoefficient> items;
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

        public async Task ShowDialogAsync(WellProductionCalculations.WellCoefficient data, int menuAction, string title)
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
            else if (menuAction == 5)
            {
                _ = await DialogService.OpenAsync<AuditTrailTable>(title,
                           new Dictionary<string, object>() { ["Id"] = data.Id, ["TableName"] = nameof(WellProductionCalculations.WellCoefficient) },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });
                return;
            }
            else if (menuAction == 6)
            {
                _ = await DialogService.OpenAsync<AuditTrailTable>(title,
                           new Dictionary<string, object>() { ["Action"] = CentralizedDatabaseSystemODataService.Affra.Core.Domain.AuditTrails.Action.Delete, ["TableName"] = nameof(WellProductionCalculations.WellCoefficient) },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });
                return;
            }
            else
            {
                response = await DialogService.OpenAsync<WellCoefficientDialog>(title,
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

        private IGenericService<WellProductionCalculations.WellCoefficient> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<WellProductionCalculations.WellCoefficient, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<WellProductionCalculations.WellCoefficient>? service = GetGenericService(serviceScope);
            var query = service.Get();

            Microsoft.OData.Client.QueryOperationResponse<WellProductionCalculations.WellCoefficient>? response = await query
                .OrderBy(x => x.LastUpdatedDate)
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<WellProductionCalculations.WellCoefficient>();

            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;

            StateHasChanged();
        }
    }
}
