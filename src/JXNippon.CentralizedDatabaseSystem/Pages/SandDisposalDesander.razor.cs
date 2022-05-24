using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.SandDisposalDesander;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class SandDisposalDesander
    {
        private SandDisposalDesanderDataGrid dataGrid;
        private SandDisposalDesanderFilterPanel filterPanel;
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
