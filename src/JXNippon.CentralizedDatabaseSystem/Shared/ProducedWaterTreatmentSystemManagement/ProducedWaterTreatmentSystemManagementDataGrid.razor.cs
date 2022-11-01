using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ProducedWaterTreatmentSystemManagement
{
    public partial class ProducedWaterTreatmentSystemManagementDataGrid
    {
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

        public DailyDataGrid<DailyProducedWaterTreatmentSystem, ProducedWaterTreatmentSystemDialog> DailyDataGrid { get; set; }

        public Task ReloadAsync()
        {
            return this.DailyDataGrid.ReloadAsync();
        }

        public Task QueryFilterAsync(IQueryable<DailyProducedWaterTreatmentSystem> query)
        {
            if (!string.IsNullOrEmpty(CommonFilter?.Search))
            {
                query = query.Where(x => x.ProducedWaterTreatmentSystemName.ToUpper().Contains(CommonFilter.Search.ToUpper()));
            }
            if (CommonFilter?.Status != null)
            {
                query = query.Where(x => x.Status.ToUpper() == CommonFilter.Status.ToUpper());
            }

            return Task.CompletedTask;
        }
    }
}
