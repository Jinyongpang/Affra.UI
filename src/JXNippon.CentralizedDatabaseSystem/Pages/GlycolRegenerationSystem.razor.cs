using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.GlycolRegenerationSystem;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class GlycolRegenerationSystem
    {
        private GlycolPumpDataGrid glycolPumpDataGrid;
        private GlycolTrainDataGrid glycolTrainDataGrid;
        private GlycolStockDataGrid glycolStockDataGrid;

        private DateFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            glycolPumpDataGrid.CommonFilter = filterPanel.CommonFilter;
            glycolTrainDataGrid.CommonFilter = filterPanel.CommonFilter;
            glycolStockDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(glycolPumpDataGrid.ReloadAsync(),
                glycolTrainDataGrid.ReloadAsync(),
                glycolStockDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(glycolPumpDataGrid.ReloadAsync(),
                glycolTrainDataGrid.ReloadAsync(),
                glycolStockDataGrid.ReloadAsync());
        }
    }
}
