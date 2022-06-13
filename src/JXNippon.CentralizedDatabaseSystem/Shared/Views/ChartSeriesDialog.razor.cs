using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ChartSeriesDialog
    {
        [Parameter] public ChartSeries Item { get; set; }
        [Parameter] public Chart Chart { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<string> groupProperties;
        private IEnumerable<string> categoryProperties;
        private IEnumerable<string> valueProperties;

        private static IEnumerable<string> markerTypes = Enum.GetValues(typeof(Radzen.Blazor.MarkerType))
            .Cast<Radzen.Blazor.MarkerType>()
            .Select(x => x.ToString())
            .ToList();

        private static IEnumerable<string> lineTypes = Enum.GetValues(typeof(Radzen.Blazor.LineType))
            .Cast<Radzen.Blazor.LineType>()
            .Select(x => x.ToString())
            .ToList();

        private static IEnumerable<string> executionTypes = Enum.GetValues(typeof(ExecutionType))
            .Cast<ExecutionType>()
            .Select(x => x.ToString())
            .ToList();

        private static HashSet<Type> valueTypes = new HashSet<Type>()
        {
            typeof(decimal?),
            typeof(decimal),
            typeof(int?),
            typeof(int),
            typeof(long?),
            typeof(long),
            typeof(double?),
            typeof(double),
        };

        private static IEnumerable<string> ChartTypes = Enum.GetValues(typeof(ChartType))
            .Cast<ChartType>()
            .Select(x => x.ToString())
            .ToList();

        private bool isAPie
        {
            get => Item?.ChartType == ChartType.PieChart
                || Item?.ChartType == ChartType.DonutChart;
        }
        private bool isViewing { get => MenuAction == 3; }

        protected override Task OnInitializedAsync()
        {
            categoryProperties = Chart.ActualType.GetProperties()
                .Select(prop => prop.Name)
                .ToList();

            valueProperties = Chart.ActualType.GetProperties()
                .Where(prop => valueTypes.Contains(prop.PropertyType))
                .Select(prop => prop.Name)
                .ToList();

            groupProperties = Chart.ActualType.GetProperties()
                .Select(prop => prop.Name)
                .ToList();

            return Task.CompletedTask;
        }

        protected Task SubmitAsync(ChartSeries arg)
        {
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
