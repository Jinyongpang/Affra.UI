using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.RollsRoyceGasEngineAndKawasakiCompressionSystems;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.RollsRoyceGasEngineAndKawasakiCompressionSystem
{
    public partial class KawasakiExportCompressorDialogPart2 : IDailyDialog<DailyKawasakiExportCompressor>
    {
        [Parameter] public DailyKawasakiExportCompressor Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }

        private bool isViewing { get => MenuAction == 3; }
    }
}
