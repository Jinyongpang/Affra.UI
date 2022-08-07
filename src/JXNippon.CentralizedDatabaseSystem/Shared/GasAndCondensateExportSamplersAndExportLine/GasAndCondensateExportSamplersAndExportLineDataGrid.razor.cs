using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GasCondensateExportSamplerAndExportLines;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.GasAndCondensateExportSamplersAndExportLine
{
    public partial class GasAndCondensateExportSamplersAndExportLineDataGrid
    {
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        public CommonFilter CommonFilter {
            get 
            { 
                return this.DailyDataGrid.CommonFilter;
            }
            set 
            { 
                this.DailyDataGrid.CommonFilter = value;
            }
        } 

        public DailyDataGrid<DailyGasCondensateExportSamplerAndExportLine, GasCondensateExportSamplerAndExportLineDialog> DailyDataGrid { get; set; }

        public Task ReloadAsync()
        {
            return this.DailyDataGrid.ReloadAsync();
        }
    }
}
