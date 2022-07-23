using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class MOCFormTemplate
    {
        [Parameter] public string RecordCode { get; set; }
        [Parameter] public string Description { get; set; }

    }
}
