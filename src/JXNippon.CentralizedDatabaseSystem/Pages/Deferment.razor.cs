using AntDesign;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Deferment : IDisposable
    {      
        private Tabs tabs;
        private bool isDisposed = false;

        [Parameter]
        public string activeKey { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        }

        private void NavigationManager_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            tabs.ActiveKey = activeKey;
        }

        private void OnActiveKeyChanged(string activeKey)
        {
            NavigationManager.NavigateTo($"deferment/{activeKey}");
        }

        void IDisposable.Dispose()
        {
            if (!isDisposed)
            {
                NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
                tabs.Dispose();
                isDisposed = true;
            }
        }
    }
}
