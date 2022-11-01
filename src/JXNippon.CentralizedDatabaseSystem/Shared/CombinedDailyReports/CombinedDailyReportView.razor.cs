using System.Collections.Generic;
using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.CoolingMediumSystem;
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
using JXNippon.CentralizedDatabaseSystem.Shared.VendorActivities;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHead;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHeadAndSeparationSystem;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportView
    {
        private bool isEditing;
        private bool isLoading = true;
        private bool isUserHavePermission = true;

        [Parameter] public CombinedDailyReport Data { get; set; }

        [Inject] private DialogService DialogService { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }

        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        private CommonFilter CommonFilter { get; set; }

        [Inject] private IUserService UserService { get; set; }

        private FPSOHelangSummaryDataGrid fpsoHelangSummaryDataGrid;
        private HIPAndLWPSummaryDataGrid hipAndLWPSummaryDataGrid;
        private LongTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid;
        private OperatingChangeDataGrid operatingChangeDataGrid;
        private MajorEquipmentStatusDataGrid majorEquipmentStatusDataGrid;
        private WellStreamCoolerDataGrid wellStreamCoolerDataGrid;
        private HIPWellHeadParameterDataGrid hipWellHeadParameterDataGrid;
        private LWPWellHeadParameterDataGrid lwpWellHeadParameterDataGrid;
        private RollsRoyceRB211EngineDataGrid rollsRoyceRB211EngineDataGrid;
        private KawasakiExportCompressorDataGrid kawasakiExportCompressorDataGrid;
        private KawasakiExportCompressorDataGridPart2 kawasakiExportCompressorDataGridPart2;
        private GlycolPumpDataGrid glycolPumpDataGrid;
        private GlycolTrainDataGrid glycolTrainDataGrid;
        private CoolingMediumSystemDataGrid coolingMediumSystemDataGrid;
        private PowerGenerationAndDistributionManagementDataGrid powerGenerationAndDistributionManagementDataGrid;
        private UtilitiesDataGrid utilitiesDataGrid;
        private WaterTankDataGrid waterTankDataGrid;
        private NitrogenGeneratorDataGrid nitrogenGeneratorDataGrid;
        private ProducedWaterTreatmentSystemManagementDataGrid producedWaterTreatmentSystemManagementDataGrid;
        private LWPActivityDataGrid lWPActivityDataGrid;
        private CommunicationSystemDataGrid communicationSystemDataGrid;
        private VendorActivitiesDataGrid vendorActivitiesDataGrid;
        private LogisticDataGrid logisticDataGrid;
        private MaximoWorkOrderDataGrid maximoWorkOrderDataGrid;

        long editId;
        private void SetIsEditing(bool value)
        {
            this.isEditing = value;
            this.StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager);
            CommonFilter.Date = this.Data.Date.UtcDateTime;

            this.isLoading = false;
            this.isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.CombinedDailyReport), HasReadPermissoin = true, HasWritePermission = true });
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
                this.Data.Status = CombinedDailyReportStatus.Approved;
                await service.UpdateAsync(this.Data, this.Data.Id);
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
                this.Data.Status = CombinedDailyReportStatus.Rejected;
                await service.UpdateAsync(this.Data, this.Data.Id);
                this.AffraNotificationService.NotifySuccess("Report rejected.");
                this.DialogService.Close();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private async Task LoadFPSOHelangSummaryDataGridAsync(LoadDataArgs args)
        {
            fpsoHelangSummaryDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadHIPAndLWPSummaryDataGridAsync(LoadDataArgs args)
        {
            hipAndLWPSummaryDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLongTermOverridesAndInhibitsOnAlarmAndOrTripDataGridAsync(LoadDataArgs args)
        {
            longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadOperatingChangeDataGridAsync(LoadDataArgs args)
        {
            operatingChangeDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadMajorEquipmentDataGridAsync(LoadDataArgs args)
        {
            majorEquipmentStatusDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadWellStreamCoolerDataGridAsync(LoadDataArgs args)
        {
            wellStreamCoolerDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadHIPWellHeadParameterDataGridAsync(LoadDataArgs args)
        {
            hipWellHeadParameterDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLWPWellHeadParameterDataGridAsync(LoadDataArgs args)
        {
            lwpWellHeadParameterDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadRollsRoyceRB211EngineDataGridAsync(LoadDataArgs args)
        {
            rollsRoyceRB211EngineDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadKawasakiExportCompressorDataGridAsync(LoadDataArgs args)
        {
            kawasakiExportCompressorDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadKawasakiExportCompressorDataGridPart2Async(LoadDataArgs args)
        {
            kawasakiExportCompressorDataGridPart2.CommonFilter = CommonFilter;
        }
        private async Task LoadGlycolPumpDataGridAsync(LoadDataArgs args)
        {
            glycolPumpDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadGlycolTrainDataGridAsync(LoadDataArgs args)
        {
            glycolTrainDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadCoolingMediumSystemDataGridAsync(LoadDataArgs args)
        {
            coolingMediumSystemDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadPowerGenerationAndDistributionManagementDataGridAsync(LoadDataArgs args)
        {
            powerGenerationAndDistributionManagementDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadUtilitiesDataGridAsync(LoadDataArgs args)
        {
            utilitiesDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadWaterTankDataGridAsync(LoadDataArgs args)
        {
            waterTankDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadNitrogenGeneratorDataGridAsync(LoadDataArgs args)
        {
            nitrogenGeneratorDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadProducedWaterTreatmentSystemManagementDataGridAsync(LoadDataArgs args)
        {
            producedWaterTreatmentSystemManagementDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLWPActivityDataGridAsync(LoadDataArgs args)
        {
            lWPActivityDataGrid.CommonFilter = CommonFilter;
        }

        private async Task LoadCommunicationSystemDataGridAsync(LoadDataArgs args)
        {
            communicationSystemDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadVendorActivitiesDataGridAsync(LoadDataArgs args)
        {
            vendorActivitiesDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLogisticDataGridAsync(LoadDataArgs args)
        {
            logisticDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadMaximoWorkOrderDataGridAsync(LoadDataArgs args)
        {
            maximoWorkOrderDataGrid.CommonFilter = CommonFilter;
        }
        private int GetTotalUnfillProperty(object property)
        {
            return property.GetType().GetProperties().Where(x => x.GetValue(property) is null).Count();
        }
    }
}
