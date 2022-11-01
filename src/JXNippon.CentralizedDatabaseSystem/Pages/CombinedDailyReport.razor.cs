using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.ChemicalInjection;
using JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.CoolingMediumSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.DailyProduction;
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
using Microsoft.OData.Client;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class CombinedDailyReport
    {
        private int focusId = -1;
        private DateTime? previousDate;
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
        private DieselDataGrid dieselDataGrid;

        [Inject] private NavigationManager NavManager { get; set; }
        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        private CommonFilter CommonFilter { get; set; }

        [Parameter]
        public bool HasFocus { get; set; }

        [Parameter]
        public EventCallback<bool> HasFocusChanged { get; set; }
        [Parameter] public CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport Item { get; set; }

        private string defaultCDRClass = "m-0 d-flex align-items-center title";
        private string emptyCDRClass = "m-0 d-flex align-items-center title text-warning";
        private string CDRClass = "";

        protected override async Task OnInitializedAsync()
        {
            CDRClass = defaultCDRClass;
            CommonFilter = new CommonFilter(NavManager);
            CommonFilter.Date = GlobalDataSource.GlobalDateFilter.Start;

            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScope);
            var query = (DataServiceQuery<CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport>)service.Get();

            var response = await query
                .Expand(x => x.DailyHealthSafetyEnvironment)
                .Expand(x => x.DailyLifeBoats)
                .Expand(x => x.DailyLongTermOverridesInhibitsOnAlarmTrips)
                .Expand(x => x.DailyOperatingChanges)
                .Expand(x => x.DailyLossOfPrimaryContainmentIncident)
                .Expand(x => x.DailyHIPAndLWPSummarys)
                .Expand(x => x.DailyFPSOHelangSummarys)
                .Expand(x => x.DailySandDisposalDesander)
                .Expand(x => x.DailyCiNalco)
                .Expand(x => x.DailyInowacInjection)
                .Expand(x => x.DailyCommunicationSystems)
                .Expand(x => x.DailyLWPActivitys)
                .Expand(x => x.DailyVendorActivitys)
                .Expand(x => x.DailyUtilitys)
                .Expand(x => x.DailyWaterTank)
                .Expand(x => x.DailyNitrogenGenerator)
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
                .Where(x => x.Date == CommonFilter.Date.ToUniversalTime())
                .ToQueryOperationResponseAsync<CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport>();

            this.Item = response.FirstOrDefault();

            await base.OnInitializedAsync();
        }
        private IGenericService<CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports.CombinedDailyReport, ICentralizedDatabaseSystemUnitOfWork>>();
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

        private async Task LoadDieselDataGridAsync(LoadDataArgs args)
        {
            dieselDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
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
                maximoWorkOrderDataGrid?.ReloadAsync() ?? Task.CompletedTask,
                dieselDataGrid?.DailyDataGrid.ReloadAsync() ?? Task.CompletedTask);
        }

        private Task OnFocusAsync(int i)
        {
            previousDate = CommonFilter.Date ?? previousDate;
            this.CommonFilter.Date = null;
            this.focusId = i;
            this.HasFocus = i != -1;
            if (this.HasFocus)
            {
                isRadzenPanelExpandedList[i] = true;
            }
            if (i == -1 && CommonFilter.Date is null)
            {
                CommonFilter.Date = previousDate;
            }

            return HasFocusChanged.InvokeAsync(i != -1);
        }

        private string GetFocusClass(int? i)
        {
            if (this.focusId == -1)
            {
                return string.Empty;
            }
            else if (this.focusId == i)
            {
                return "focused-cdr";
            }
            else
            {
                return "hidden-card";
            }
        }

        private string GetTabFocusClass(int? i)
        {
            return this.focusId == i
                ? "focused-tab"
                : string.Empty;
        }

        private bool CheckIsFocused(int i)
        {
            return this.focusId == i;
        }
    }
}
