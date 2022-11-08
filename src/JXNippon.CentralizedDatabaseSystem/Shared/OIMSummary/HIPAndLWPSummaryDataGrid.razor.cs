using System.Collections.ObjectModel;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.OIMSummary
{
    public partial class HIPAndLWPSummaryDataGrid
    {
        [Parameter] public Collection<DailyHIPAndLWPSummary> Data { get; set; }
        [Parameter] public CombinedDailyReport CombinedDailyReport { get; set; }
        [Parameter] public EventCallback<Collection<DailyHIPAndLWPSummary>> DataChanged { get; set; }
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

        public DailyDataGrid<DailyHIPAndLWPSummary, HIPAndLWPSummaryDialog> DailyDataGrid { get; set; }

        public Task ReloadAsync()
        {
            return this.DailyDataGrid.ReloadAsync();
        }

        private Task OnDataChangedAsync(Collection<DailyHIPAndLWPSummary> data)
        {
            var result = new List<DailyHIPAndLWPSummary>();
            if (this.ReportDate is not null
                && this.CombinedDailyReport is not null
                && this.CombinedDailyReport.DailySK10Production is not null
                && this.CombinedDailyReport.DailySK10Production.SK10CumulativeGasExport is not null)
            {
                result.Add(new()
                { 
                    Date = this.CombinedDailyReport.Date,
                    Remark = this.CalculateSummary(),
                });

                result.Add(new()
                {
                    Date = this.CombinedDailyReport.Date,
                    Remark = this.CombinedDailyReport.DailySK10Production.SK10CumulativeGasExport > 0
                        ? "Refer to section 18 below for details on vendor activities reports."
                        : "Refer to shutdown daily progress report",
                });
            }
            foreach (var item in data)
            {
                result.Add(item);
            }
            data = new Collection<DailyHIPAndLWPSummary>(result);
            return DataChanged.InvokeAsync(data);
        }

        private string CalculateSummary()
        {
            var sk10Production = this.CombinedDailyReport.DailySK10Production;
            if (sk10Production.SK10CumulativeGasExport > 0)
            {
                return "SK-10 cumulative Gas Export of "
                    + $"{sk10Production.SK10CumulativeGasExport}"
                    + $" MMscf (HIP: "
                    + $"{this.CombinedDailyReport.DailyHIPProduction?.HIPCumulativeGasExport ?? 0}"
                    + " MMscf, FPSO Helang: "
                    + $"{this.CombinedDailyReport.DailyFPSOHelangProduction?.FPSOCumulativeGasExport ?? 0}"
                    + " MMscf) was "
                    + $"{this.GetCondition()}"
                    + " than SK-10 GODC "
                    + $"{this.CombinedDailyReport.DailyHIPProduction?.HIPCumulativeGasExportRemark?.Trim()}"
                    + " plan of "
                    + $"{this.CombinedDailyReport.DailyHIPProduction?.HIPGasExportGODCPlan}"
                    + " MMscfd";
            }
            else if (sk10Production.SK10CumulativeGasExport == 0)
            {
                return "SK-10 Helang remain zero production";
            }
            return string.Empty;
        }

        private string GetCondition()
        {
            var sk10GasExport = this.CombinedDailyReport.DailySK10Production?.SK10CumulativeGasExport ?? 0;
            var hipGasExporPlan = this.CombinedDailyReport.DailyHIPProduction?.HIPGasExportGODCPlan ?? 0;
            var difference = sk10GasExport - hipGasExporPlan;
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
            else if(difference > 0)
            {
                return "higher";
            }
            return "Equal";
        }
    }
}
