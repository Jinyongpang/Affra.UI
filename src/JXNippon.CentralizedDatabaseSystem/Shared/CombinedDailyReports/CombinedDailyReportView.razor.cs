using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.ChemicalInjection;
using JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.CoolingMediumSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.DailyProduction;
using JXNippon.CentralizedDatabaseSystem.Shared.GlycolRegenerationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.HealthSafetyAndEnvironment;
using JXNippon.CentralizedDatabaseSystem.Shared.LivingQuartersUtilitiesAndOthers;
using JXNippon.CentralizedDatabaseSystem.Shared.Logistic;
using JXNippon.CentralizedDatabaseSystem.Shared.LWPActivity;
using JXNippon.CentralizedDatabaseSystem.Shared.MajorEquipment;
using JXNippon.CentralizedDatabaseSystem.Shared.MaximoWorkOrder;
using JXNippon.CentralizedDatabaseSystem.Shared.OIMSummary;
using JXNippon.CentralizedDatabaseSystem.Shared.PowerGenerationAndDistributionManagement;
using JXNippon.CentralizedDatabaseSystem.Shared.ProducedWaterTreatmentSystemManagement;
using JXNippon.CentralizedDatabaseSystem.Shared.RollsRoyceGasEngineAndKawasakiCompressionSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.SandDisposalDesander;
using JXNippon.CentralizedDatabaseSystem.Shared.VendorActivities;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHead;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHeadAndSeparationSystem;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportView
    {
        private bool isEditing;
        private bool isLoading = true;
        private bool isUserHavePermission = true;

        [Parameter] public CombinedDailyReport Item { get; set; }

        [Inject] private DialogService DialogService { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }

        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        private CommonFilter CommonFilter { get; set; }

        [Inject] private IUserService UserService { get; set; }

        private FPSOHelangSummaryDataGrid fpsoHelangSummaryDataGrid;

        long editId;
        private void SetIsEditing(bool value)
        { 
            this.isEditing = value;
            this.StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager);
            CommonFilter.Date = this.Item.Date.UtcDateTime;

            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScope);
            var query = (DataServiceQuery<CombinedDailyReport>)service.Get();

            var response = await query
                .Expand(x => x.DailyHealthSafetyEnvironment)
                .Expand(x => x.DailyLifeBoat)
                .Expand(x => x.DailyLongTermOverridesInhibitsOnAlarmTrips)
                .Expand(x => x.DailyOperatingChanges)
                .Expand(x => x.DailyLossOfPrimaryContainmentIncident)
                .Expand(x => x.DailyHIPAndLWPSummarys)
                .Expand(x => x.DailySandDisposalDesander)
                .Expand(x => x.DailyCiNalco)
                .Expand(x => x.DailyInowacInjection)
                .Expand(x => x.DailyCommunicationSystems)
                .Expand(x => x.DailyLWPActivitys)
                .Expand(x => x.DailyVendorActivitys)
                .Expand(x => x.DailyUtilitys)
                .Expand(x => x.DailyWaterTanks)
                .Expand(x => x.DailyNitrogenGenerators)
                .Expand(x => x.DailyMaximoWorkOrders)
                .Expand(x => x.DailyAnalysisResult)
                .Expand(x => x.DailyCoolingMediumSystems)
                .Expand(x => x.DailyLogistics)
                .Expand(x => x.DailyGlycolPumps)
                .Expand(x => x.DailyGlycolTrains)
                .Expand(x => x.DailyGlycolStock)
                .Expand(x => x.DailyKawasakiExportCompressors)
                .Expand(x => x.DailyRollsRoyceRB211Engines)
                .Expand(x => x.DailyHIPWellHeadParameters)
                .Expand(x => x.DailyLWPWellHeadParameters)
                .Expand(x => x.DailyGasCondensateExportSamplerAndExportLine)
                .Expand(x => x.DailyWellHeadAndSeparationSystem)
                .Expand(x => x.DailyWellStreamCoolers)
                .Expand(x => x.DailySK10Production)
                .Expand(x => x.DailyHIPProduction)
                .Expand(x => x.DailyFPSOHelangProduction)
                .Expand(x => x.DailyMajorEquipmentStatuses)
                .Expand(x => x.DailyDiesel)
                .Expand(x => x.DailyProducedWaterTreatmentSystems)
                .Expand(x => x.DailyDeOilerInjection)
                .Expand(x => x.DailyPowerGenerationAndDistributions)
                .Where(x => x.Date == this.Item.Date)
                .ToQueryOperationResponseAsync<CombinedDailyReport>();

            this.Item = response.FirstOrDefault();
            this.isLoading = false;
            this.isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.CombinedDailyReport), HasReadPermissoin = true, HasWritePermission = true});
            await base.OnInitializedAsync();
        }

        private IGenericService<CombinedDailyReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<CombinedDailyReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task ApproveAsync()
        {
            try
            {
                using var scope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(scope);
                this.Item.Status = CombinedDailyReportStatus.Approved;
                await service.UpdateAsync(this.Item, this.Item.Id);
                this.AffraNotificationService.NotifySuccess("Report approved.");
                this.DialogService.Close();
            }
            catch (Exception ex)
            { 
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private async Task RejectAsync()
        {
            try
            {
                using var scope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(scope);
                this.Item.Status = CombinedDailyReportStatus.Rejected;
                await service.UpdateAsync(this.Item, this.Item.Id);
                this.AffraNotificationService.NotifySuccess("Report rejected.");
                this.DialogService.Close();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }
        void startEdit(long id)
        {
            editId = id;
        }

        private async Task stopEdit()
        {
            try
            {
                using var scope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(scope);
                await service.UpdateAsync(this.Item, this.Item.Id);

                this.AffraNotificationService.NotifySuccess("Record edited.");

                StateHasChanged();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }

            editId = -1;
        }
        private async Task LoadFPSOHelangSummaryDataGridAsync(LoadDataArgs args)
        {
            fpsoHelangSummaryDataGrid.CommonFilter = CommonFilter;
        }
    }
}
