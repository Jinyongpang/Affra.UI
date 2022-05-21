using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.OIMSummary;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class OIMSummary
    {
        private HIPAndLWPSummaryDataGrid hipAndLWPSummaryDataGrid;
        private FPSOHelangSummaryDataGrid fpsoHelangSummaryDataGrid;

        private DateFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            hipAndLWPSummaryDataGrid.CommonFilter = filterPanel.CommonFilter;
            fpsoHelangSummaryDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(hipAndLWPSummaryDataGrid.ReloadAsync(),
                fpsoHelangSummaryDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(hipAndLWPSummaryDataGrid.ReloadAsync(),
                fpsoHelangSummaryDataGrid.ReloadAsync());
        }
    }
}
