using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHeadAndSeparationSystem;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class WellHeadAndSeparationSystem
    {
        private WellHeadAndSeparationSystemDataGrid dataGrid;
        private WellHeadAndSeparationSystemFilterPanel filterPanel;
        private WellStreamCoolerDataGrid wellStreamCoolerDataGrid;
        private async Task LoadDataAsync(LoadDataArgs args)
        {
            dataGrid.CommonFilter = filterPanel.CommonFilter;
            wellStreamCoolerDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(dataGrid.ReloadAsync(),
                wellStreamCoolerDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(dataGrid.ReloadAsync(),
                wellStreamCoolerDataGrid.ReloadAsync());
        }
    }
}
