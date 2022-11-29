using System.Collections.ObjectModel;
using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ChartDialog
    {
        [Parameter] public View View { get; set; }
        [Parameter] public Column Column { get; set; }
        public Chart Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        
        private static IEnumerable<string> LegendPositionTypes = Enum.GetValues(typeof(LegendPosition))
           .Cast<LegendPosition>()
           .Select(x => x.ToString())
           .ToList();

        private IEnumerable<string> types;
        private bool isViewing { get => MenuAction == 3; }
        private int current { get; set; } = 0;

        private AntDesign.Steps steps;

        private string[] dateFiltersId;
       
        protected override Task OnInitializedAsync()
        {
            Column.ViewName = View.Name;
            Column.View = View;
            types = ViewService.GetTypeMapping()
                .Select(x => x.Key)
                .ToHashSet();

            Item = new Chart()
            {
                ChartSeries = new Collection<ChartSeries>(),
            };
            if (!string.IsNullOrEmpty(Column.ColumnComponent))
            {
                Item = JsonSerializer.Deserialize<Chart>(Column.ColumnComponent) ?? Item;
            }

            var filters = View.Rows.SelectMany(x => x.Columns)
                .Where(x => x.ComponentType == nameof(DateFilter))
                .Select(x => JsonSerializer.Deserialize<DateFilter>(x.ColumnComponent).Id)
                .ToList();

            filters.Add(Constants.Constant.GlobalDateFilter);

            dateFiltersId = filters.Distinct()
                .ToArray();

            return Task.CompletedTask;
        }

        protected Task SubmitAsync(Chart arg)
        {
            if (this.current < 2)
            {
                this.MovePage(1);
            }
            else
            {
                Column.ColumnComponent = JsonSerializer.Serialize(Item);
                DialogService.Close(true);
            }
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }

        private void OnChange(object value)
        {
            var row = value as Row;
            Column.RowId = row.Id;
            Column.Row = row;
            Column.Sequence = row.Columns.Count;
        }

        private void MovePage(int i)
        {
            this.current += i;
        }
    }
}
