using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.ChemicalInjection;
using Radzen;


namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class ChemicalInjection
    {
        private CINalcoDataGrid ciNalcoDataGrid;
        private InowacInjectionDataGrid inowacDataGrid;

        private DateFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            ciNalcoDataGrid.CommonFilter = filterPanel.CommonFilter;
            inowacDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(ciNalcoDataGrid.ReloadAsync(),
                inowacDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(ciNalcoDataGrid.ReloadAsync(),
                inowacDataGrid.ReloadAsync());
        }
    }
}
