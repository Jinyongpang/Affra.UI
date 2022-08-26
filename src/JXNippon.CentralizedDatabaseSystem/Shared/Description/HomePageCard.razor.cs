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
        
        [Parameter] public EventCallback<MouseEventArgs> OnButtonClick { get; set; }
        [Parameter] public string SearchKeys { get; set; }

        [Parameter] public string Search { get; set; }

        public bool IsVisible
        {
            get 
            {
                return this.CheckIsVisible(this.Search);
            }
        }

        public bool CheckIsVisible(string search)
        {
            return search is null
               || this.SearchKeys.Contains(search, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
