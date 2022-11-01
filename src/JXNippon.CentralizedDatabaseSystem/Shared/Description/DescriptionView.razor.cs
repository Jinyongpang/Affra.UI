using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class DescriptionView
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
    }
}
