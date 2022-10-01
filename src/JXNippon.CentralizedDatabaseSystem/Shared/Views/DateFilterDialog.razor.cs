using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class DateFilterDialog
    {
        [Parameter] public View View { get; set; }
        [Parameter] public Column Column { get; set; }
        [Parameter] public int MenuAction { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        public DateFilter Item { get; set; }

        private bool isViewing { get => MenuAction == 3; }
        private int current { get; set; } = 0;
        private AntDesign.Steps steps;


        private static readonly IEnumerable<string> types = Enum.GetValues(typeof(DateFilterType))
            .Cast<DateFilterType>()
            .Select(x => x.ToString())
            .ToList();

        protected override Task OnInitializedAsync()
        {
            Column.ViewName = View.Name;
            Column.View = View;

            Item = new DateFilter()
            {
                IsSimpleCard = true,
            };
            if (!string.IsNullOrEmpty(Column.ColumnComponent))
            {
                Item = JsonSerializer.Deserialize<DateFilter>(Column.ColumnComponent) ?? Item;
            }

            return Task.CompletedTask;
        }

        protected Task SubmitAsync(DateFilter arg)
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
    }
}
