using JXNippon.CentralizedDatabaseSystem.Shared.Description;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class HomeLegacy : IDisposable
    {
        private bool isDisposed = false;
        private bool isSearchEmpty = false;
        private string search;
        private List<HomePageCard> homePageCards = new();
        [Inject] private NavigationManager navigationManager { get; set; }

        private HomePageCard homePageCardRef
        {
            set
            {
                homePageCards.Add(value);
            }
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
        }

        protected override async Task OnInitializedAsync()
        {
        }

        private void OnSearchChange(string value)
        {
            this.search = value;
            this.isSearchEmpty = homePageCards.All(x => !x.CheckIsVisible(value));
            this.StateHasChanged();
        }


        private void Navigate(string path)
        {
            navigationManager.NavigateTo(path);
        }
    }
}
