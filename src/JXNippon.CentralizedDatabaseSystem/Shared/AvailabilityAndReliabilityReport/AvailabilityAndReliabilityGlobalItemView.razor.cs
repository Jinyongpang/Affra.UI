using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class AvailabilityAndReliabilityGlobalItemView
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public int? Year { get; set; }
        [Parameter] public string Title { get; set; }
    }
}
