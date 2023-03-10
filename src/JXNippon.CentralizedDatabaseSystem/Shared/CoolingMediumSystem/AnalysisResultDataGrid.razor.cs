using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CoolingMediumSystem
{
    public partial class AnalysisResultDataGrid
    {
        [Parameter] public Collection<DailyAnalysisResult> Data { get; set; }
        [Parameter] public EventCallback<Collection<DailyAnalysisResult>> DataChanged { get; set; }
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        [Parameter] public DateTimeOffset? ReportDate { get; set; }

        public DailyDataGrid<DailyAnalysisResult, AnalysisResultDialog> DailyDataGrid { get; set; }
    }
}