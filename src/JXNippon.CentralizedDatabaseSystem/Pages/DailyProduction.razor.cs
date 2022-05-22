using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.DailyProduction;
using Radzen;


namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class DailyProduction
    {
        private ProductionSK10DataGrid sk10DataGrid;
        private ProductionHIPDataGrid hipDataGrid;
        private ProductionFPSOHelangDataGrid fpsoDataGrid;

        private DailyProductionFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            sk10DataGrid.CommonFilter = filterPanel.CommonFilter;
            hipDataGrid.CommonFilter = filterPanel.CommonFilter;
            fpsoDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(sk10DataGrid.ReloadAsync(),
                hipDataGrid.ReloadAsync(),
                fpsoDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(sk10DataGrid.ReloadAsync(),
                hipDataGrid.ReloadAsync(),
                fpsoDataGrid.ReloadAsync());
        }
    }
}
