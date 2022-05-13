using JXNippon.CentralizedDatabaseSystem.Models;
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

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(powerGenerationAndDistributionManagementDataGrid.ReloadAsync(),
                producedWaterTreatmentSystemManagementDataGrid.ReloadAsync(),
                deOilerInjectionDataGrid.ReloadAsync(),
                majorEquipmentStatusDataGrid.ReloadAsync());
        }

    }
}
