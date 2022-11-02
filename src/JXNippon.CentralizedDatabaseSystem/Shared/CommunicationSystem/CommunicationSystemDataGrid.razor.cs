using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CommunicationSystems;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem
{
    public partial class CommunicationSystemDataGrid
    {
        [Parameter] public Collection<DailyCommunicationSystem> Data { get; set; }
        [Parameter] public EventCallback<Collection<DailyCommunicationSystem>> DataChanged { get; set; }
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        [Parameter] public DateTimeOffset? ReportDate { get; set; }

        public DailyDataGrid<DailyCommunicationSystem, CommunicationSystemDialog> DailyDataGrid { get; set; }
    }
}
