using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DailyProduction
{
    public partial class DailyProductionFilterPanel
    {
        [Parameter] public EventCallback<CommonFilter> Change { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        public CommonFilter CommonFilter { get; set; }
        protected override Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager);
            return Task.CompletedTask;
        }
        private async Task OnChangeAsync(object value)
        {
            CommonFilter.AppendQuery(NavManager);
            await Change.InvokeAsync(CommonFilter);
        }
    }
}
