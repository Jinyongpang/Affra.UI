using System.Linq.Dynamic.Core;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.SCEElements;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.SCEElement
{
    public partial class SCEElementGrid
    {
        private readonly AntDesign.Menu menu;
        private readonly string search;
        private bool isLoading = false;
        public string SCEFilter { get; set; }
        private RadzenDataGrid<SCEElementRecord> grid;
        private IEnumerable<SCEElementRecord> items = new List<SCEElementRecord>();
        public IEnumerable<SCEElementGroupRecord> groupItems = new List<SCEElementGroupRecord>();
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        public int Count { get; set; }
        private IGenericService<SCEElementRecord> GetGenericSCEService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<SCEElementRecord, IManagementOfChangeUnitOfWork>>();
        }
        public Task ReloadAsync()
        {
            return grid.FirstPage(true);
        }
        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            StateHasChanged();

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<SCEElementRecord>? SCEService = GetGenericSCEService(serviceScope);
            var query = SCEService.Get();

            if (SCEFilter != null && SCEFilter != "All")
            {
                var status = Int16.Parse(SCEFilter);
                query = query.Where(x => x.SCEGroupId == status);
            }

            Microsoft.OData.Client.QueryOperationResponse<SCEElementRecord>? response = await query
                .OrderBy(x => x.SCECode)
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<SCEElementRecord>();


            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;

            StateHasChanged();
        }
        public async Task ShowDialogAsync(SCEElementRecord data, int menuAction, string title)
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
                    var service = GetGenericSCEService(serviceScope);
                    await service.DeleteAsync(data);

                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<SCEElementDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction }, { "GroupItem", groupItems } },
                           Constant.DialogOptions);

                if (response == true)
                {
                    try
                    {
                        using var serviceScope = ServiceProvider.CreateScope();
                        var service = GetGenericSCEService(serviceScope);

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
