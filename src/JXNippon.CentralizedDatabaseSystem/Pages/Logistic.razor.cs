using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Logistic
    {
        private LogisticDataGrid dataGrid;
        private LogisticFilterPanel filterPanel;
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
