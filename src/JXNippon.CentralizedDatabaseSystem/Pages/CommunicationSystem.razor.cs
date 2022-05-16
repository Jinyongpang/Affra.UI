using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class CommunicationSystem
    {
        private CommunicationSystemDataGrid dataGrid;
        private DateFilterPanel filterPanel;
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
