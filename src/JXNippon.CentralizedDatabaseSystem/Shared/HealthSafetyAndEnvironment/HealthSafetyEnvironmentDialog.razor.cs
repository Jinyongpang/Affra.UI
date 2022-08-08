using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.HealthSafetyAndEnvironment
{
    public partial class HealthSafetyEnvironmentDialog : IDailyDialog<DailyHealthSafetyEnvironment>
    {
        [Parameter] public DailyHealthSafetyEnvironment Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }

        private bool isViewing { get => MenuAction == 3; }

    }
}
