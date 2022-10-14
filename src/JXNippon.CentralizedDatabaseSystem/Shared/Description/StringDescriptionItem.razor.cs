using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class StringDescriptionItem
    {
        [Parameter] public bool IsEditing { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public int? Span { get; set; }
    }
}
