using System.Linq.Dynamic.Core;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CoolingMediumSystem
{
    public partial class CoolingMediumSystemDataGrid
    {
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        [Parameter] public DateTimeOffset? ReportDate { get; set; }

        public DailyDataGrid<DailyCoolingMediumSystem, CoolingMediumSystemDialog> DailyDataGrid { get; set; }
        private Task QueryFilterAsync(IQueryable<DailyCoolingMediumSystem> query)
        {
            if (DailyDataGrid.CommonFilter is not null)
            {
                if (!string.IsNullOrEmpty(DailyDataGrid.CommonFilter.Search))
                {
                    query = query.Where(x => x.CoolingMediumSystemName.ToUpper().Contains(DailyDataGrid.CommonFilter.Search.ToUpper()));
                }
                if (DailyDataGrid.CommonFilter.Status != null)
                {
                    query = query.Where(x => x.Status.ToUpper() == DailyDataGrid.CommonFilter.Status.ToUpper());
                }
            }
            return Task.CompletedTask;
        }
    }
}
