using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class AvailabilityAndReliabilityTableView
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public int Year { get; set; }
    }
}
