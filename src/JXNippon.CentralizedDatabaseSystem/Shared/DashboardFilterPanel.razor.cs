using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class DashboardFilterPanel
    {
        [Parameter] public EventCallback<CommonFilter> Change { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        public CommonFilter CommonFilter { get; set; }


        protected override async Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager);
            CommonFilter.Date = DateTime.Today.AddDays(-1);
        }

        private async Task OnChangeAsync(object value)
        {
            CommonFilter.AppendQuery(NavManager);
            await Change.InvokeAsync(CommonFilter);
        }
    }
}
