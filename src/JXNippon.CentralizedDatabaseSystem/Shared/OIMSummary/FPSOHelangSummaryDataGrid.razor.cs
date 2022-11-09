using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries;
using JXNippon.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.OIMSummary
{
    public partial class FPSOHelangSummaryDataGrid
    {
        [Parameter] public Collection<DailyFPSOHelangSummary> Data { get; set; }
        [Parameter] public CombinedDailyReport CombinedDailyReport { get; set; }
        [Parameter] public EventCallback<Collection<DailyFPSOHelangSummary>> DataChanged { get; set; }
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        [Parameter] public DateTimeOffset? ReportDate { get; set; }
        [Inject] private ICombinedDailyReportService CombinedDailyReportService { get; set; }

        public CommonFilter CommonFilter
        {
            get
            {
                return DailyDataGrid.CommonFilter;
            }
            set
            {
                DailyDataGrid.CommonFilter = value;
            }
        }

        public DailyDataGrid<DailyFPSOHelangSummary, FPSOHelangSummaryDialog> DailyDataGrid { get; set; }

        public Task ReloadAsync()
        {
            return DailyDataGrid.ReloadAsync();
        }

        private Task OnDataChangedAsync(Collection<DailyFPSOHelangSummary> data)
        {
            if (ReportDate is not null)
            {
                data = CombinedDailyReportService.AppendSummary(data, CombinedDailyReport);
            }
            return DataChanged.InvokeAsync(data);
        }
    }
}
