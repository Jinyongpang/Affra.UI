using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.MajorEquipment;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class MajorEquipmentStatus
    {
        private MajorEquipmentStatusDataGrid dataGrid;
        private DieselDataGrid dieselDataGrid;
        private MajorEquipmentStatusFilterPanel filterPanel;
        private DateFilterPanel dateFilterPanel;
        private async Task LoadDataAsync(LoadDataArgs args)
        {
            dataGrid.CommonFilter = filterPanel.CommonFilter;
            dieselDataGrid.DailyDataGrid.CommonFilter = dateFilterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(dataGrid.ReloadAsync(), dieselDataGrid.DailyDataGrid.ReloadAsync());
        }
    }
}
