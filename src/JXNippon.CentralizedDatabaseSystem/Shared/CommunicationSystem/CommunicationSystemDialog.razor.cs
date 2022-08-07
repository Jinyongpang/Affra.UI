using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Templates;
using CommunicationSystems = CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CommunicationSystems;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem
{
    public partial class CommunicationSystemDialog : IDailyDialog<CommunicationSystems.DailyCommunicationSystem>
    {
        [Parameter] public CommunicationSystems.DailyCommunicationSystem Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }

        private bool isViewing { get => MenuAction == 3; }
    }
}
