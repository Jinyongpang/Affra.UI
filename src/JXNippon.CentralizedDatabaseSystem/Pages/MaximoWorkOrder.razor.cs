using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.MaximoWorkOrder;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class MaximoWorkOrder
    {
        private MaximoWorkOrderDataGrid dataGrid;
        private MaximoWorkOrderFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            dataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(dataGrid.ReloadAsync());
        }
    }
}
