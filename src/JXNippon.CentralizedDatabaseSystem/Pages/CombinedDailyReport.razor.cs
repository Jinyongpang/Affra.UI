﻿using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.ChemicalInjection;
using JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.CoolingMediumSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.DailyProduction;
using JXNippon.CentralizedDatabaseSystem.Shared.Dashboard;
using JXNippon.CentralizedDatabaseSystem.Shared.GasAndCondensateExportSamplersAndExportLine;
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
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class CombinedDailyReport
    {
        private readonly bool[] isRadzenPanelExpandedList = new bool[20];
        private PowerGenerationAndDistributionManagementDataGrid powerGenerationAndDistributionManagementDataGrid;
        private ProducedWaterTreatmentSystemManagementDataGrid producedWaterTreatmentSystemManagementDataGrid;
        private MajorEquipmentStatusDataGrid majorEquipmentStatusDataGrid;
        private DeOilerInjectionDataGrid deOilerInjectionDataGrid;
        private ProductionSK10DataGrid productionSK10DataGrid;
        private ProductionHIPDataGrid productionHIPDataGrid;
        private ProductionFPSOHelangDataGrid productionFPSOHelangDataGrid;
        private WellHeadAndSeparationSystemDataGrid wellHeadAndSeparationSystemDataGrid;
        private WellStreamCoolerDataGrid wellStreamCoolerDataGrid;
        private LogisticDataGrid logisticDataGrid;
        private LWPActivityDataGrid lWPActivityDataGrid;
        private CommunicationSystemDataGrid communicationSystemDataGrid;
        private HIPWellHeadParameterDataGrid hipWellHeadParameterDataGrid;
        private LWPWellHeadParameterDataGrid lwpWellHeadParameterDataGrid;
        private RollsRoyceRB211EngineDataGrid rollsRoyceRB211EngineDataGrid;
        private KawasakiExportCompressorDataGrid kawasakiExportCompressorDataGrid;
        private FPSOHelangSummaryDataGrid fpsoHelangSummaryDataGrid;
        private HIPAndLWPSummaryDataGrid hipAndLWPSummaryDataGrid;
        private HealthSafetyAndEnvironmentDataGrid healthSafetyAndEnvironmentDataGrid;
        private LossOfPrimaryContainmentIncidentDataGrid lossOfPrimaryContainmentIncidentDataGrid;
        private LifeBoatsDataGrid lifeBoatsDataGrid;
        private LongTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid;
        private OperatingChangeDataGrid operatingChangeDataGrid;
        private CINalcoDataGrid ciNalcoDataGrid;
        private InowacInjectionDataGrid inowacInjectionDataGrid;
        private GlycolPumpDataGrid glycolPumpDataGrid;
        private GlycolTrainDataGrid glycolTrainDataGrid;
        private GlycolStockDataGrid glycolStockDataGrid;
        private GasAndCondensateExportSamplersAndExportLineDataGrid gasAndCondensateExportSamplersDataGrid;
        private AnalysisResultDataGrid analysisResultDataGrid;
        private CoolingMediumSystemDataGrid coolingMediumSystemDataGrid;
        private UtilitiesDataGrid UtilitiesDataGrid;
        private WaterTankDataGrid waterTankDataGrid;
        private NitrogenGeneratorDataGrid nitrogenGeneratorDataGrid;
        private SandDisposalDesanderDataGrid sandDisposalDesanderDataGrid;
        private VendorActivitiesDataGrid vendorActivitiesDataGrid;
        private MaximoWorkOrderDataGrid maximoWorkOrderDataGrid;

        [Inject] private NavigationManager NavManager { get; set; }
        private CommonFilter CommonFilter { get; set; }

        protected override Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager);
            CommonFilter.Date = DateTime.Today;

            return Task.CompletedTask;
        }

        private async Task OnChangeAsync(object value)
        {
            CommonFilter.AppendQuery(NavManager);
            await this.ReloadAsync();
        }

        private async Task LoadPowerGenerationAndDistributionManagementDataGridAsync(LoadDataArgs args)
        {
            powerGenerationAndDistributionManagementDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadProducedWaterTreatmentSystemManagementDataGridAsync(LoadDataArgs args)
        {
            producedWaterTreatmentSystemManagementDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadDeOilerInjectionDataGridAsync(LoadDataArgs args)
        {
            deOilerInjectionDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadMajorEquipmentDataGridAsync(LoadDataArgs args)
        {
            majorEquipmentStatusDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadProductionSK10DataGridAsync(LoadDataArgs args)
        {
            productionSK10DataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadProductionHIPDataGridAsync(LoadDataArgs args)
        {
            productionHIPDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadProductionFPSODataGridAsync(LoadDataArgs args)
        {
            productionFPSOHelangDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadWellHeadAndSeparationSystemDataGridAsync(LoadDataArgs args)
        {
            wellHeadAndSeparationSystemDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadWellStreamCoolerDataGridAsync(LoadDataArgs args)
        {
            wellStreamCoolerDataGrid.CommonFilter = CommonFilter;
        }

        private async Task LoadLogisticDataGridAsync(LoadDataArgs args)
        {
            logisticDataGrid.CommonFilter = CommonFilter;
        }

        private async Task LoadLWPActivityDataGridAsync(LoadDataArgs args)
        {
            lWPActivityDataGrid.CommonFilter = CommonFilter;
        }

        private async Task LoadCommunicationSystemDataGridAsync(LoadDataArgs args)
        {
            communicationSystemDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
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
        private async Task LoadFPSOHelangSummaryDataGridAsync(LoadDataArgs args)
        {
            fpsoHelangSummaryDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadHIPAndLWPSummaryDataGridAsync(LoadDataArgs args)
        {
            hipAndLWPSummaryDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadHealthSafetyAndEnvironmentDataGridAsync(LoadDataArgs args)
        {
            healthSafetyAndEnvironmentDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLossOfPrimaryContainmentIncidentDataGridAsync(LoadDataArgs args)
        {
            lossOfPrimaryContainmentIncidentDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLifeBoatsDataGridAsync(LoadDataArgs args)
        {
            lifeBoatsDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLongTermOverridesAndInhibitsOnAlarmAndOrTripDataGridAsync(LoadDataArgs args)
        {
            longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadOperatingChangeDataGridAsync(LoadDataArgs args)
        {
            operatingChangeDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadCINalcoDataGridAsync(LoadDataArgs args)
        {
            ciNalcoDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadInowacInjectionDataGridAsync(LoadDataArgs args)
        {
            inowacInjectionDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadGlycolPumpDataGridAsync(LoadDataArgs args)
        {
            glycolPumpDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadGlycolTrainDataGridAsync(LoadDataArgs args)
        {
            glycolTrainDataGrid.CommonFilter = CommonFilter;
        }

        private async Task LoadGlycolStockDataGridAsync(LoadDataArgs args)
        {
            glycolStockDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadGasAndCondensateExportSamplersAndExportLineDataGridAsync(LoadDataArgs args)
        {
            gasAndCondensateExportSamplersDataGrid.CommonFilter = CommonFilter;
        }

        private async Task LoadAnalysisResultDataGridAsync(LoadDataArgs args)
        {
            analysisResultDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadCoolingMediumSystemDataGridAsync(LoadDataArgs args)
        {
            coolingMediumSystemDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadUtilitiesDataGridAsync(LoadDataArgs args)
        {
            UtilitiesDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadWaterTankDataGridAsync(LoadDataArgs args)
        {
            waterTankDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadNitrogenGeneratorDataGridAsync(LoadDataArgs args)
        {
            nitrogenGeneratorDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadSandDisposalDesanderDataGridAsync(LoadDataArgs args)
        {
            sandDisposalDesanderDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadVendorActivitiesDataGridAsync(LoadDataArgs args)
        {
            vendorActivitiesDataGrid.CommonFilter = CommonFilter;
        }

        private async Task LoadMaximoWorkOrderDataGridAsync(LoadDataArgs args)
        {
            maximoWorkOrderDataGrid.CommonFilter = CommonFilter;
        }
        private Task OnChangeAsync(CommonFilter commonFilter)
        {
            return this.ReloadAsync();
        }

        private Task ReloadAsync()
        {
            return Task.WhenAll(powerGenerationAndDistributionManagementDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                producedWaterTreatmentSystemManagementDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                deOilerInjectionDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                majorEquipmentStatusDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                productionSK10DataGrid?.DailyDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                productionHIPDataGrid?.DailyDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                productionFPSOHelangDataGrid?.DailyDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                wellHeadAndSeparationSystemDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                wellStreamCoolerDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                logisticDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                lWPActivityDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                communicationSystemDataGrid?.DailyDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                lwpWellHeadParameterDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                hipWellHeadParameterDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                rollsRoyceRB211EngineDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                kawasakiExportCompressorDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                fpsoHelangSummaryDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                lwpWellHeadParameterDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                hipAndLWPSummaryDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                healthSafetyAndEnvironmentDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                lossOfPrimaryContainmentIncidentDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                lifeBoatsDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                operatingChangeDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                ciNalcoDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                inowacInjectionDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                glycolPumpDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                glycolStockDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                glycolTrainDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                gasAndCondensateExportSamplersDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                analysisResultDataGrid?.DailyDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                coolingMediumSystemDataGrid?.DailyDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                UtilitiesDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                waterTankDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                nitrogenGeneratorDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                sandDisposalDesanderDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                vendorActivitiesDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                maximoWorkOrderDataGrid?.ReloadAsync() ?? Task.CompletedTask);
        }
    }
}
