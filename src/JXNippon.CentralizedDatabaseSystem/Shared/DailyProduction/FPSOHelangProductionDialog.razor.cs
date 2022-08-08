using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DailyProduction
{
    public partial class FPSOHelangProductionDialog : IDailyDialog<DailyFPSOHelangProduction>
    {
        [Parameter] public DailyFPSOHelangProduction Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }

        private bool isViewing { get => MenuAction == 3; }
    }
}
