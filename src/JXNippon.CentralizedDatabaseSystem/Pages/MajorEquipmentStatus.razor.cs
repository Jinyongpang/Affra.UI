using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.MajorEquipment;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class MajorEquipmentStatus
    {
        private MajorEquipmentStatusDataGrid dataGrid;
        private MajorEquipmentStatusFilterPanel filterPanel;
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
