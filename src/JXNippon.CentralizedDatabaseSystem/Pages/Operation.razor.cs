using AntDesign;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Operation
    {
        private string activeKey = string.Empty;
        private Tabs tabs;

        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.activeKey = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToLowerInvariant();
            NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        }

        private void NavigationManager_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {           
            this.activeKey = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToLowerInvariant();
            tabs.ActiveKey = activeKey;
        }

        private void OnActiveKeyChanged(string activeKey)
        {
            NavigationManager.NavigateTo(activeKey);
        }
    }
}
