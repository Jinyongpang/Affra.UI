using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Commons
{
    public partial class OperatingChangeDailyDataGrid<TDialog> : DailyDataGrid<DailyOperatingChange, TDialog>
        where TDialog : ComponentBase, IDailyDialog<DailyOperatingChange>
    {
        protected override Task LoadDataAsync(LoadDataArgs args)
        {
            return base.LoadDataAsync(args);
        }
    }
}
