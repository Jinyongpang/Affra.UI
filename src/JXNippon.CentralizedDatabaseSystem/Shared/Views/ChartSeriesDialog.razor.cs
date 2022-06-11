using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
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

        private bool isViewing { get => MenuAction == 3; }

        protected override Task OnInitializedAsync()
        {
            categoryProperties = Chart.ActualType.GetProperties()
                .Where(prop => prop.PropertyType == typeof(DateTime)
                    || prop.PropertyType == typeof(DateTime?))
                .Select(prop => prop.Name)
                .ToList();

            valueProperties = Chart.ActualType.GetProperties()
                .Where(prop => valueTypes.Contains(prop.PropertyType))
                .Select(prop => prop.Name)
                .ToList();

            groupProperties = Chart.ActualType.GetProperties()
                .Where(prop => prop.PropertyType == typeof(string))
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
