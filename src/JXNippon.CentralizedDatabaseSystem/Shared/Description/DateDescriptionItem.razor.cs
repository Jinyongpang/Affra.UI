using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class DateDescriptionItem
    {
        [Parameter] public bool IsEditing { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public DateTimeOffset? Value { get; set; }

        [Parameter] public int? Span { get; set; }

        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }

        private string GetStyle()
        {
            if (Value is null)
            {
                return "color: #FF9F29;";
            }
            return "display: none;";
        }
    }
}
