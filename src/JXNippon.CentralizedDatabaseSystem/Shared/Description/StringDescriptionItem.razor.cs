using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class StringDescriptionItem<TItem>
        where TItem : class
    {
        [Parameter] public bool IsEditable { get; set; } = true;

        [Parameter] public string Title { get; set; }

        [Parameter] public string? Value { get; set; }

        [Parameter] public int? Span { get; set; }
        [Parameter] public bool IsRequired { get; set; } = true;

        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        [Parameter] public TItem Item { get; set; }
        [Parameter] public long ItemId { get; set; }

        [Parameter] public EventCallback<string?> ValueChanged { get; set; }
    }
}
