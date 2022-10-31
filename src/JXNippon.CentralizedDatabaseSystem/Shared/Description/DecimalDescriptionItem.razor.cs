using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class DecimalDescriptionItem<TItem>
        where TItem : class
    {
        [Parameter] public bool IsEditing { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public decimal? Value { get; set; }

        [Parameter] public int? Span { get; set; }

        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        [Parameter] public TItem Item { get; set; }
        [Parameter] public long ItemId { get; set; }

        [Parameter] public EventCallback<decimal?> ValueChanged { get; set; }

        private string GetStyle()
        {
            return this.Value is null
                ? "background-color: yellow;"
                : null;
        }
    }
}
