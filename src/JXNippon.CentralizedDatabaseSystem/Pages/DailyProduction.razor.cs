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
            sk10DataGrid.DailyDataGrid.CommonFilter = filterPanel.CommonFilter;
            hipDataGrid.DailyDataGrid.CommonFilter = filterPanel.CommonFilter;
            fpsoDataGrid.DailyDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(sk10DataGrid.DailyDataGrid.ReloadAsync(),
                hipDataGrid.DailyDataGrid.ReloadAsync(),
                fpsoDataGrid.DailyDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(sk10DataGrid.DailyDataGrid.ReloadAsync(),
                hipDataGrid.DailyDataGrid.ReloadAsync(),
                fpsoDataGrid.DailyDataGrid.ReloadAsync());
        }
    }
}
