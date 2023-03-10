using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.ProducedWaterTreatmentSystemManagement;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class ProducedWaterTreatmentSystemManagement
    {
        private ProducedWaterTreatmentSystemManagementDataGrid dataGrid;
        private DeOilerInjectionDataGrid deOilerInjectionDataGrid;
        private ProducedWaterTreatmentSystemManagementFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            dataGrid.CommonFilter = filterPanel.CommonFilter;
            deOilerInjectionDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(dataGrid.ReloadAsync(),
                deOilerInjectionDataGrid.ReloadAsync());
        }
    }
}
