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
        public string Mode { get; set; }

        public CommonFilter(NavigationManager navigationManager)
        {
            this.Search = navigationManager.GetQueryString<string>(nameof(CommonFilter.Search));
            this.Date = navigationManager.GetQueryString<DateTime?>(nameof(CommonFilter.Date));
            this.Status = navigationManager.GetQueryString<string>(nameof(CommonFilter.Status));
            this.Mode = navigationManager.GetQueryString<string>(nameof(CommonFilter.Mode));
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
            if (this.Mode != null)
            {
                queries.Add(nameof(CommonFilter.Mode), this.Mode);
            }
            var uriBuilder = new UriBuilder(navigationManager.Uri);
            navigationManager.NavigateTo(QueryHelpers.AddQueryString(uriBuilder.Uri.AbsolutePath, queries));
        }
    }
}
