using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.HealthSafetyAndEnvironment
{
    public partial class OperatingChangeDataGrid
    {
        [Parameter] public Collection<DailyOperatingChange> Data { get; set; }
        [Parameter] public EventCallback<Collection<DailyOperatingChange>> DataChanged { get; set; }
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        [Parameter] public DateTimeOffset? ReportDate { get; set; }
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

        public DailyDataGrid<DailyOperatingChange, OperatingChangeDialog> DailyDataGrid { get; set; }

        public Task ReloadAsync()
        {
            return DailyDataGrid.ReloadAsync();
        }
    }
}
