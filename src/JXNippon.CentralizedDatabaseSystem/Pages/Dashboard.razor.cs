﻿using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
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
                lWPActivityDataGrid.ReloadAsync());
        }
    }
}
