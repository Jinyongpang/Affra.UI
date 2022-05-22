using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHead;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class WellHead
    {
        private HIPWellHeadParameterDataGrid hipWellHeadParameterDataGrid;
        private LWPWellHeadParameterDataGrid lwpWellHeadParameterDataGrid;

        private WellHeadFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            hipWellHeadParameterDataGrid.CommonFilter = filterPanel.CommonFilter;
            lwpWellHeadParameterDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(hipWellHeadParameterDataGrid.ReloadAsync(),
                lwpWellHeadParameterDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(hipWellHeadParameterDataGrid.ReloadAsync(),
                lwpWellHeadParameterDataGrid.ReloadAsync());
        }
    }
}
