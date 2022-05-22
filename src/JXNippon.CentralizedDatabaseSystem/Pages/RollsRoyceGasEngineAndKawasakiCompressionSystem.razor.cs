using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.RollsRoyceGasEngineAndKawasakiCompressionSystem;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class RollsRoyceGasEngineAndKawasakiCompressionSystem
    {
        private RollsRoyceRB211EngineDataGrid rollsRoyceRB211EngineDataGrid;
        private KawasakiExportCompressorDataGrid kawasakiExportCompressorDataGrid;

        private DateFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            rollsRoyceRB211EngineDataGrid.CommonFilter = filterPanel.CommonFilter;
            kawasakiExportCompressorDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(rollsRoyceRB211EngineDataGrid.ReloadAsync(),
                kawasakiExportCompressorDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(rollsRoyceRB211EngineDataGrid.ReloadAsync(),
                kawasakiExportCompressorDataGrid.ReloadAsync());
        }
    }
}
