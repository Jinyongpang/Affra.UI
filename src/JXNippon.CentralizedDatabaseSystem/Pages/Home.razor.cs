using JXNippon.CentralizedDatabaseSystem.Shared.Description;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Home : IDisposable
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

        private void OnSearchChange()
        {
            isSearchEmpty = homePageCards.All(x => !x.Visible);
        }

        private bool IsVisibleSearch(string contains)
        {
            return this.search is null
                || contains.Contains(this.search, StringComparison.InvariantCultureIgnoreCase);
        }

        private void Navigate(string path)
        {
            navigationManager.NavigateTo(path);
        }
    }
}
