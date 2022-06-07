using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewComponent
    {
        [Parameter] public View View { get; set; }

        [Parameter] public DateTimeOffset? StartDate { get; set; }

        [Parameter] public DateTimeOffset? EndDate { get; set; }

        private List<ChartComponent> chartComponents = new();
        public ChartComponent chartComponentRef { set => chartComponents.Add(value); }

        public Task ReloadAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {

            StartDate = startDate ?? StartDate;
            EndDate = endDate ?? EndDate;
            StateHasChanged();
            return Task.WhenAll(chartComponents.Select(x => x.ReloadAsync(StartDate, EndDate)).ToList());
        }
    }
}
