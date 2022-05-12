using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class PowerGenerationAndDistributionManagement
    {
        private PowerGenerationAndDistributionManagementDataGrid dataGrid;
        private DeOilerInjectionDataGrid deOilerInjectionDataGrid;
        private PowerGenerationAndDistributionManagementFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            dataGrid.CommonFilter = filterPanel.CommonFilter;
            deOilerInjectionDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll( dataGrid.ReloadAsync(),
                deOilerInjectionDataGrid.ReloadAsync());

        }
    }
}
