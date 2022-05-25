using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.VendorActivities;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class VendorActivities
    {
        private VendorActivitiesDataGrid dataGrid;
        private VendorActivitiesFilterPanel filterPanel;
        private async Task LoadDataAsync(LoadDataArgs args)
        {
            dataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await dataGrid.ReloadAsync();
        }
    }
}
