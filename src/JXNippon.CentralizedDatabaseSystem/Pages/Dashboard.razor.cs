using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.Logistic;
using JXNippon.CentralizedDatabaseSystem.Shared.OIMSummary;
using JXNippon.CentralizedDatabaseSystem.Shared.Dashboard;
using JXNippon.CentralizedDatabaseSystem.Shared.MajorEquipment;
using JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.DailyProduction;
using JXNippon.CentralizedDatabaseSystem.Shared.RollsRoyceGasEngineAndKawasakiCompressionSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHeadAndSeparationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHead;
using JXNippon.CentralizedDatabaseSystem.Shared.PowerGenerationAndDistributionManagement;
using JXNippon.CentralizedDatabaseSystem.Shared.ProducedWaterTreatmentSystemManagement;
using JXNippon.CentralizedDatabaseSystem.Shared.FileManagement;
using JXNippon.CentralizedDatabaseSystem.Shared.LWPActivity;
using JXNippon.CentralizedDatabaseSystem.Shared.HealthSafetyAndEnvironment;
using JXNippon.CentralizedDatabaseSystem.Shared.ChemicalInjection;
using JXNippon.CentralizedDatabaseSystem.Shared.GlycolRegenerationSystem;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Dashboard
    {
        private DashboardFilterPanel filterPanel;
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

        private async Task LoadPowerGenerationAndDistributionManagementDataGridAsync(LoadDataArgs args)
        {
            powerGenerationAndDistributionManagementDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadProducedWaterTreatmentSystemManagementDataGridAsync(LoadDataArgs args)
        {
            producedWaterTreatmentSystemManagementDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadDeOilerInjectionDataGridAsync(LoadDataArgs args)
        {
            deOilerInjectionDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadMajorEquipmentDataGridAsync(LoadDataArgs args)
        {
            majorEquipmentStatusDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadProductionSK10DataGridAsync(LoadDataArgs args)
        {
            productionSK10DataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadProductionHIPDataGridAsync(LoadDataArgs args)
        {
            productionHIPDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadProductionFPSODataGridAsync(LoadDataArgs args)
        {
            productionFPSOHelangDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadWellHeadAndSeparationSystemDataGridAsync(LoadDataArgs args)
        {
            wellHeadAndSeparationSystemDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadWellStreamCoolerDataGridAsync(LoadDataArgs args)
        {
            wellStreamCoolerDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task LoadLogisticDataGridAsync(LoadDataArgs args)
        {
            logisticDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task LoadLWPActivityDataGridAsync(LoadDataArgs args)
        {
            lWPActivityDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task LoadCommunicationSystemDataGridAsync(LoadDataArgs args)
        {
            communicationSystemDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadHIPWellHeadParameterDataGridAsync(LoadDataArgs args)
        {
            hipWellHeadParameterDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadLWPWellHeadParameterDataGridAsync(LoadDataArgs args)
        {
            lwpWellHeadParameterDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadRollsRoyceRB211EngineDataGridAsync(LoadDataArgs args)
        {
            rollsRoyceRB211EngineDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadKawasakiExportCompressorDataGridAsync(LoadDataArgs args)
        {
            kawasakiExportCompressorDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadFPSOHelangSummaryDataGridAsync(LoadDataArgs args)
        {
            fpsoHelangSummaryDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadHIPAndLWPSummaryDataGridAsync(LoadDataArgs args)
        {
            hipAndLWPSummaryDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadHealthSafetyAndEnvironmentDataGridAsync(LoadDataArgs args)
        {
            healthSafetyAndEnvironmentDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadLossOfPrimaryContainmentIncidentDataGridAsync(LoadDataArgs args)
        {
            lossOfPrimaryContainmentIncidentDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadLifeBoatsDataGridAsync(LoadDataArgs args)
        {
            lifeBoatsDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadLongTermOverridesAndInhibitsOnAlarmAndOrTripDataGridAsync(LoadDataArgs args)
        {
            longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadOperatingChangeDataGridAsync(LoadDataArgs args)
        {
            operatingChangeDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadCINalcoDataGridAsync(LoadDataArgs args)
        {
            ciNalcoDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadInowacInjectionDataGridAsync(LoadDataArgs args)
        {
            inowacInjectionDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadGlycolPumpDataGridAsync(LoadDataArgs args)
        {
            glycolPumpDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadGlycolTrainDataGridAsync(LoadDataArgs args)
        {
            glycolTrainDataGrid.CommonFilter = filterPanel.CommonFilter;
        }
        private async Task LoadGlycolStockDataGridAsync(LoadDataArgs args)
        {
            glycolStockDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private Task OnChangeAsync(CommonFilter commonFilter)
        {
            return this.ReloadAsync();
        }

        private Task ReloadAsync()
        {
            return Task.WhenAll(powerGenerationAndDistributionManagementDataGrid.ReloadAsync(),
                producedWaterTreatmentSystemManagementDataGrid.ReloadAsync(),
                deOilerInjectionDataGrid.ReloadAsync(),
                majorEquipmentStatusDataGrid.ReloadAsync(),
                productionSK10DataGrid.ReloadAsync(),
                productionHIPDataGrid.ReloadAsync(),
                productionFPSOHelangDataGrid.ReloadAsync(),
                wellHeadAndSeparationSystemDataGrid.ReloadAsync(),
                wellStreamCoolerDataGrid.ReloadAsync(),
                logisticDataGrid.ReloadAsync(),
                lWPActivityDataGrid.ReloadAsync(),
                communicationSystemDataGrid.ReloadAsync(),
                lwpWellHeadParameterDataGrid.ReloadAsync(),
                hipWellHeadParameterDataGrid.ReloadAsync(),
                rollsRoyceRB211EngineDataGrid.ReloadAsync(),
                kawasakiExportCompressorDataGrid.ReloadAsync(),
                fpsoHelangSummaryDataGrid.ReloadAsync(),
                lwpWellHeadParameterDataGrid.ReloadAsync(),
                hipAndLWPSummaryDataGrid.ReloadAsync(),
                healthSafetyAndEnvironmentDataGrid.ReloadAsync(),
                lossOfPrimaryContainmentIncidentDataGrid.ReloadAsync(),
                lifeBoatsDataGrid.ReloadAsync(),
                longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid.ReloadAsync(),
                operatingChangeDataGrid.ReloadAsync(),
                ciNalcoDataGrid.ReloadAsync(),
                inowacInjectionDataGrid.ReloadAsync(),
                glycolPumpDataGrid.ReloadAsync(),
                glycolStockDataGrid.ReloadAsync(),
                glycolTrainDataGrid.ReloadAsync());
        }
    }
}
