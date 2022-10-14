using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class DateDescriptionItem
    {
        [Parameter] public bool IsEditing { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public DateTimeOffset? Value { get; set; }

        [Parameter] public int? Span { get; set; }
    }
}
