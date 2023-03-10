using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.LivingQuartersUtilitiesAndOthers
{
    public partial class WaterTankDialog : IDailyDialog<DailyWaterTank>
    {
        [Parameter] public DailyWaterTank Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }

        private bool isViewing { get => MenuAction == 3; }
    }
}
