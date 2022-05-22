using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.CoolingMediumSystem;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class CoolingMediumSystem
    {
        private AnalysisResultDataGrid dataGrid;
        private CoolingMediumSystemFilterPanel filterPanel;
        private CoolingMediumSystemDataGrid coolingMediumSystemDataGrid;
        private async Task LoadDataAsync(LoadDataArgs args)
        {
            dataGrid.CommonFilter = filterPanel.CommonFilter;
            coolingMediumSystemDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(dataGrid.ReloadAsync(),
                coolingMediumSystemDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(dataGrid.ReloadAsync(),
                coolingMediumSystemDataGrid.ReloadAsync());
        }
    }
}
