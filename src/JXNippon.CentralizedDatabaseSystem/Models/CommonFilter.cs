using JXNippon.CentralizedDatabaseSystem.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace JXNippon.CentralizedDatabaseSystem.Models
{
    public class CommonFilter
    {
        public string Search { get; set; }

        public string Status { get; set; }

        public DateTime? Date { get; set; }

        public CommonFilter(NavigationManager navigationManager)
        {
            this.Search = navigationManager.GetQueryString<string>(nameof(CommonFilter.Search));
            this.Date = navigationManager.GetQueryString<DateTime?>(nameof(CommonFilter.Date));
            this.Status = navigationManager.GetQueryString<string>(nameof(CommonFilter.Status));
        }

        public void AppendQuery(NavigationManager navigationManager)
        {
            var queries = new Dictionary<string, string>();
            if (this.Search != null)
            {
                queries.Add(nameof(CommonFilter.Search), this.Search);
            }
            if (this.Status != null)
            {
                queries.Add(nameof(CommonFilter.Status), this.Status);
            }
            if (this.Date != null)
            {
                queries.Add(nameof(CommonFilter.Date), this.Date.Value.ToString("yyyy-MM-dd"));
            }
            var uriBuilder = new UriBuilder(navigationManager.Uri);
            navigationManager.NavigateTo(QueryHelpers.AddQueryString(uriBuilder.Uri.AbsolutePath, queries));
        }
    }
}
