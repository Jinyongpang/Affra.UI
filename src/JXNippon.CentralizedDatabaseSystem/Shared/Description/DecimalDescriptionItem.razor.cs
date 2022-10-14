using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class DecimalDescriptionItem
    {
        [Parameter] public bool IsEditing { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public decimal? Value { get; set; }

        [Parameter] public int? Span { get; set; }
    }
}
