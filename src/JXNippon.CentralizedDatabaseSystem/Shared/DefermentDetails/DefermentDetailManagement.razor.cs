using Affra.Core.Domain.Services;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.SCEElements;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class DefermentDetailManagement
    {
        private const string All = "All";
        private Menu menu;
        private string search;
        private DefermentDetailGrid defermentDetailGrid { get; set; }
        private List<string> defermentDetailStatusList = new List<string>();
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        private IGenericService<DefermentDetail> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DefermentDetail, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        protected override Task OnInitializedAsync()
        {
            return this.LoadDefermentStatus();
        }
        private async Task LoadDefermentStatus()
        {
            foreach (var val in Enum.GetValues(typeof(DefermentDetailStatus)).Cast<DefermentDetailStatus>())
            {
                defermentDetailStatusList.Add(val.ToString());
            }

            await InitDefermentDetail();
        }
        private Task InitDefermentDetail()
        {
            if(defermentDetailGrid.DefermentDetailFilter == null)
                defermentDetailGrid.DefermentDetailFilter = All;

            return defermentDetailGrid.ReloadAsync();
        }
        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            defermentDetailGrid.DefermentDetailFilter = menuItem.Key;
            return defermentDetailGrid.ReloadAsync();
        }
    }
}
