using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class DefermentTableRow
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Unit { get; set; }
    }
}
