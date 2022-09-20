using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Commons
{
    public partial class PelaIcon
    {
        [Parameter] public string Type { get; set; }
        [Parameter] public string Class { get; set; }
        [Parameter] public string Style { get; set; }

    }
}
