using System.Collections.ObjectModel;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries;
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
        
        public CommonFilter CommonFilter
        {
            get
            {
                return this.DailyDataGrid.CommonFilter;
            }
            set
            {
                this.DailyDataGrid.CommonFilter = value;
            }
        }

        public DailyDataGrid<DailyFPSOHelangSummary, FPSOHelangSummaryDialog> DailyDataGrid { get; set; }

        public Task ReloadAsync()
        {
            return this.DailyDataGrid.ReloadAsync();
        }

        private Task OnDataChangedAsync(Collection<DailyFPSOHelangSummary> data)
        {
            var result = new List<DailyFPSOHelangSummary>();
            if (this.ReportDate is not null
                && this.CombinedDailyReport is not null
                && this.CombinedDailyReport.DailyFPSOHelangProduction is not null
                && this.CombinedDailyReport.DailyFPSOHelangProduction.FPSOCumulativeGasExport is not null)
            {
                result.Add(new()
                {
                    Date = this.CombinedDailyReport.Date,
                    Remark = this.CalculateSummary(),
                });
            }
            foreach (var item in data)
            {
                result.Add(item);
            }
            data = new Collection<DailyFPSOHelangSummary>(result);
            return DataChanged.InvokeAsync(data);
        }

        private string CalculateSummary()
        {
            var fpsoProduction = this.CombinedDailyReport.DailyFPSOHelangProduction;
            if (fpsoProduction.FPSOCumulativeGasExport > 0)
            {
                return $"<a>FPSO Helang cumulative Gas Export of "
                    + $"<a style=\"background-color:#FEFB00;\">{fpsoProduction.FPSOCumulativeGasExport}</a>"
                    + " MMscf was "
                    + $"<a style=\"background-color:#02F900;\">{this.GetCondition()}</a>"
                    + " than GODC "
                    + $"<a style=\"background-color:#00FDFF;\">{this.CombinedDailyReport.DailyFPSOHelangProduction?.FPSOCumulativeGasExportRemark}</a>"
                    + " plan of "
                    + $"<a style=\"background-color:#FF40FF;\">{this.CombinedDailyReport.DailyFPSOHelangProduction?.FPSOGasExportGODCPlan}</a>"
                    + " MMscf. FPSO daily cumulative Crude Oil into storage tank was "
                    + $"<a style=\"background-color:#FF2800;\">{this.CombinedDailyReport.DailyFPSOHelangProduction?.FPSOCumulativeCrudeOil}</a>"
                    + " bbls.</a>";
            }
            else if (fpsoProduction.FPSOCumulativeGasExport == 0)
            {
                return "FPSO Helang shutdown";
            }
            return string.Empty;
        }

        private string GetCondition()
        {
            var fpsoGasExport = this.CombinedDailyReport.DailyFPSOHelangProduction?.FPSOCumulativeGasExport ?? 0;
            var fpsoGasExportPlan = this.CombinedDailyReport.DailyFPSOHelangProduction?.FPSOGasExportGODCPlan ?? 0;
            var difference = fpsoGasExport - fpsoGasExportPlan;
            if (difference < 0 && difference > -1.0m)
            {
                return "Slightly lower";
            }
            else if (difference < 0)
            {
                return "lower";
            }
            else if (difference > 0 && difference < 1.0m)
            {
                return "Slightly higher";
            }
            else if (difference > 0)
            {
                return "higher";
            }
            return "Equal";
        }
    }
}
