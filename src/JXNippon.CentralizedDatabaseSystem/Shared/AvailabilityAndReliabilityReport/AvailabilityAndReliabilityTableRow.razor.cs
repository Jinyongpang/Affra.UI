using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class AvailabilityAndReliabilityTableRow
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string RowName { get; set; }
    }
}
