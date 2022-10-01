using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using JXNippon.CentralizedDatabaseSystem.Domain.Statistics;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class StatisticsDialog
    {
        private static readonly IEnumerable<string> compareTypes = Enum.GetValues(typeof(StatisticsCompareType))
            .Cast<StatisticsCompareType>()
            .Select(x => x.ToString())
            .ToList();
        private IEnumerable<string> types;
        private bool isViewing { get => MenuAction == 3; }
        private int current { get; set; } = 0;
        private AntDesign.Steps steps;
        private string[] dateFiltersId;
        private IEnumerable<string> properties;

        [Parameter] public View View { get; set; }
        [Parameter] public Column Column { get; set; }
        public Statistic Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        protected override Task OnInitializedAsync()
        {
            Column.ViewName = View.Name;
            Column.View = View;
            types = ViewService.GetTypeMapping()
                .Select(x => x.Key)
                .ToHashSet();

            Item = new Statistic()
            {
                ColorGreater = "#38E54D",
                ColorLesser = "#FF884B",
                ColorEqual = "#256D85",
                IsSimpleCard = true,
            };

            if (!string.IsNullOrEmpty(Column.ColumnComponent))
            {
                Item = JsonSerializer.Deserialize<Statistic>(Column.ColumnComponent) ?? Item;
            }

            var filters = View.Rows.SelectMany(x => x.Columns)
                .Where(x => x.ComponentType == nameof(DateFilter))
                .Select(x => JsonSerializer.Deserialize<DateFilter>(x.ColumnComponent).Id)
                .ToList();

            filters.Add(Constants.Constant.GlobalDateFilter);

            dateFiltersId = filters.Distinct()
                .ToArray();

            this.RefreshTypeProperties();

            return Task.CompletedTask;
        }

        protected Task SubmitAsync(Statistic arg)
        {
            if (current < 1)
            {
                MovePage(1);
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
            current += i;
        }

        private void RefreshTypeProperties()
        {
            Type type = Item.ActualType;

            if (type is null)
            {
                return;
            }

            properties = type.GetProperties()
                .Where(p => p.Name != "Date")
                .Select(prop => prop.Name)
                .ToList();
        }
    }
}
