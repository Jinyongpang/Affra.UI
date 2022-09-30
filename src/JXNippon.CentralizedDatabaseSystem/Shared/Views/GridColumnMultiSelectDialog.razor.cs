using System.Text;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class GridColumnMultiSelectDialog
    {
        [Parameter] public Grid Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<string> Types { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private Table<GridColumn> table;

        private IEnumerable<string> properties;

        private ICollection<GridColumn> gridColumns = new List<GridColumn>();

        private bool isViewing { get => MenuAction == 3; }

        private string overrideType;

        private int pageSize;

        private int count;

        private string editId;

        private string editColumn;

        private IEnumerable<GridColumn> selectedRows;

        protected override Task OnInitializedAsync()
        {
            this.RefreshTypeProperties();
            return Task.CompletedTask;
        }

        protected Task SubmitAsync(Grid arg)
        {
            DialogService.Close(this.selectedRows);
            return Task.CompletedTask;
        }

        private void StartEdit(string id, string property)
        {
            editId = id;
            editColumn = property;
        }

        private void StopEdit()
        {
            editId = null;
            editColumn = null;
        }


        private void Cancel()
        {
            DialogService.Close(null);
        }

        private void RefreshTypeProperties()
        {

            Type type = string.IsNullOrEmpty(this.overrideType)
                    ? this.Item.ActualType
                    : System.Type.GetType(ViewHelper.GetTypeMapping()[this.overrideType]);

            properties = type.GetProperties()
                .Where(p => p.Name != "Date")
                .Select(prop => prop.Name)
                .ToList();

            gridColumns = properties
                .Select(prop => new GridColumn 
                { 
                    Property = prop,
                    Title = GetTitle(prop),
                    FormatString = prop.Contains("Date") ? "{0:d}" : string.Empty,
                    ConditionalStylings = new List<ConditionalStyling>(),
                    Type = this.overrideType,
                })
                .ToList();

            this.pageSize = gridColumns.Count;
            this.count = gridColumns.Count;
        }

        private string GetTitle(string input)
        {
            var stringBuilder = new StringBuilder();
            bool isPreviousCap = true;
            foreach (char c in input)
            {
                if (char.IsUpper(c) && !isPreviousCap)
                { 
                    stringBuilder.Append(' ');
                }
                isPreviousCap = char.IsUpper(c);
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Trim();
        }
    }
}
