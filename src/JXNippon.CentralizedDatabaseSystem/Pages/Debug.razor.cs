using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Logistics;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Charts;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Debug
    {
        private GenericDataGrid<DailyLogistic> dataGrid;
        private IEnumerable<ChartSeries> chartSeries = new List<ChartSeries>()
        {
            new ()
            {
                Title = "Today's Level",
                CategoryProperty = "DateUI",
                LineType = "Dashed",
                MarkerType = "Circle",
                Smooth = false,
                ValueProperty = "TodayLevel"
            },
        };
        private async Task LoadDataAsync(LoadDataArgs args)
        {
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await dataGrid.ReloadAsync();
        }
    }
}
