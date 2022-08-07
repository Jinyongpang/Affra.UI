using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Commons
{
    public interface IDailyDialog<TItem>
        where TItem : class, IExtras
    {
        [Parameter] IEnumerable<CustomColumn> CustomColumns { get; set; }
        [Parameter] TItem Item { get; set; }
        [Parameter] int MenuAction { get; set; }

        bool IsViewing { get => MenuAction == 3; }
    }
}
