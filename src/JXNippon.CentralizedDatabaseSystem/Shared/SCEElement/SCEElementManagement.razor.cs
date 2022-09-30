using System.Linq.Dynamic.Core;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.SCEElements;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.SCEElement
{
    public partial class SCEElementManagement
    {
        private const string All = "All";
        private Menu menu;
        private readonly string search;
        private IEnumerable<SCEElementGroupRecord> sceElementGroupList = new List<SCEElementGroupRecord>();
        private SCEElementGrid sceElementGrid { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        private IGenericService<SCEElementGroupRecord> GetGenericSCEGroupService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<SCEElementGroupRecord, IManagementOfChangeUnitOfWork>>();
        }
        protected override Task OnInitializedAsync()
        {
            return LoadSCEGroupAsync();
        }
        private async Task LoadSCEGroupAsync()
        {
            StateHasChanged();

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<SCEElementGroupRecord>? SCEGroupService = GetGenericSCEGroupService(serviceScope);
            var query = SCEGroupService.Get();

            var SCEResponse = await query
                .ToQueryOperationResponseAsync<SCEElementGroupRecord>();

            sceElementGroupList = SCEResponse
                .OrderBy(x => x.Name)
                .ToList();

            StateHasChanged();

            await InitSCEElement();
        }
        private Task InitSCEElement()
        {
            if (sceElementGrid.SCEFilter == null)
            {
                sceElementGrid.SCEFilter = "All";
            }

            sceElementGrid.groupItems = sceElementGroupList;
            return sceElementGrid.ReloadAsync();
        }
        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            sceElementGrid.SCEFilter = menuItem.Key;
            return sceElementGrid.ReloadAsync();
        }
        public async Task ShowGroupDialogAsync(SCEElementGroupRecord data, string title)
        {
            dynamic? response;

            response = await DialogService.OpenAsync<SCEElementGroupDialog>(title,
                           new Dictionary<string, object>() { { "Item", data } },
                           Constant.DialogOptions);

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = GetGenericSCEGroupService(serviceScope);

                    if (data.Id > 0)
                    {
                        await service.UpdateAsync(data, data.Id);
                        AffraNotificationService.NotifyItemUpdated();
                    }
                    else
                    {
                        await service.InsertAsync(data);
                        AffraNotificationService.NotifyItemCreated();
                    }

                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
            }

            await LoadSCEGroupAsync();
        }
        public async Task ShowGroupDeleteDialogAsync(SCEElementGroupRecord data, string title)
        {
            dynamic? response;
            response = await DialogService.OpenAsync<GenericConfirmationDialog>(title,
                        new Dictionary<string, object>() { },
                        new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

            if (response == true)
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var service = GetGenericSCEGroupService(serviceScope);
                await service.DeleteAsync(data);

                AffraNotificationService.NotifyItemDeleted();
            }

            await LoadSCEGroupAsync();
        }
    }
}
