using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.LivingQuartersUtilitiesAndOthers;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class LivingQuartersUtilitiesAndOthers
    {
        private UtilitiesDataGrid utilitiesDataGrid;
        private NitrogenGeneratorDataGrid nitrogenGeneratorDataGrid;
        private WaterTankDataGrid waterTankDataGrid;

        private LivingQuartersUtilitiesAndOtherFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            utilitiesDataGrid.CommonFilter = filterPanel.CommonFilter;
            nitrogenGeneratorDataGrid.CommonFilter = filterPanel.CommonFilter;
            waterTankDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(utilitiesDataGrid.ReloadAsync(),
                nitrogenGeneratorDataGrid.ReloadAsync(),
                waterTankDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(utilitiesDataGrid.ReloadAsync(),
                nitrogenGeneratorDataGrid.ReloadAsync(),
                waterTankDataGrid.ReloadAsync());
        }
    }
}
