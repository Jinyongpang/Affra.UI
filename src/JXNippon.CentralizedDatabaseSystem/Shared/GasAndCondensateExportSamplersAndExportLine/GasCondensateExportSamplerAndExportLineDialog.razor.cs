using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GasCondensateExportSamplerAndExportLines;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.GasAndCondensateExportSamplersAndExportLine
{
    public partial class GasCondensateExportSamplerAndExportLineDialog : IDailyDialog<DailyGasCondensateExportSamplerAndExportLine>
    {
        [Parameter] public DailyGasCondensateExportSamplerAndExportLine Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }

        private bool isViewing { get => MenuAction == 3; }
    }
}
