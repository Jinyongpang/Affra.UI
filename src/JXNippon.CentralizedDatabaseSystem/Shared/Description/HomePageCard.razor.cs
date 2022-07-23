using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class HomePageCard
    {
        [Parameter] public string Description { get; set; }
        [Parameter] public ImageFile ImageFile { get; set; }
        [Parameter] public string Class { get; set; }
        [Parameter] public string ButtonText { get; set; }
        [Parameter] public bool Visible { get; set; } = true;
        [Parameter] public EventCallback<MouseEventArgs> OnButtonClick { get; set; }
    }
}
