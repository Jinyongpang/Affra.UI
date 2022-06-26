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


        private List<DataGridComponent> gridComponents = new();
        public DataGridComponent gridComponentRef { set => gridComponents.Add(value); }

        public Task ReloadAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {

            StartDate = startDate ?? StartDate;
            EndDate = endDate ?? EndDate;
            StateHasChanged();
            List<Task> tasks = new List<Task>();
            tasks.AddRange(chartComponents.Select(x => x.ReloadAsync(StartDate, EndDate)).ToList());
            tasks.AddRange(gridComponents.Select(x => x.ReloadAsync(StartDate, EndDate)).ToList());
            return Task.WhenAll(tasks);
        }
    }
}
