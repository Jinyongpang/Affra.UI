using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.GasAndCondensateExportSamplersAndExportLine;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class GasAndCondensateExportSamplerAndExportLine
    {
        private GasAndCondensateExportSamplersAndExportLineDataGrid gasAndCondensateExportSamplersAndExportLineDataGrid;

        private DateFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            gasAndCondensateExportSamplersAndExportLineDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await gasAndCondensateExportSamplersAndExportLineDataGrid.ReloadAsync();
        }
        private async Task ReloadAsync()
        {
            await gasAndCondensateExportSamplersAndExportLineDataGrid.ReloadAsync();
        }
    }
}
