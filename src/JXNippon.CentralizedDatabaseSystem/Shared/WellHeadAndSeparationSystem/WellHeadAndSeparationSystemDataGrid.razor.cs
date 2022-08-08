using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.WellHeadAndSeparationSystem
{
    public partial class WellHeadAndSeparationSystemDataGrid
    {
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
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

        public DailyDataGrid<DailyWellHeadAndSeparationSystem, WellHeadAndSeparationSystemDialog> DailyDataGrid { get; set; }

        public Task ReloadAsync()
        {
            return this.DailyDataGrid.ReloadAsync();
        }

        public Task QueryFilterAsync(IQueryable<DailyWellHeadAndSeparationSystem> query)
        {
            if (CommonFilter != null)
            {
                if (CommonFilter.Status != null)
                {
                    query = query.Where(x => x.SandFilterS0400Status.ToUpper() == CommonFilter.Status.ToUpper());
                }
            }
            return Task.CompletedTask;
        }
    }
}
