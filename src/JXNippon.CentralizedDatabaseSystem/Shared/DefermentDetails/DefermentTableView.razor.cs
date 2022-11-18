using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class DefermentTableView
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public int Year { get; set; }
        [Parameter] public string ReportName { get; set; }
    }
}
