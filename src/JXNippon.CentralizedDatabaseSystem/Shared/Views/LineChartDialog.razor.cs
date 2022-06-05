using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class LineChartDialog
    {
        [Parameter] public View View { get; set; }
        [Parameter] public LineChart Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<string> types;
        private bool isViewing { get => MenuAction == 3; }

        protected override Task OnInitializedAsync()
        {
            Item.ViewName = View.Name;
            Item.View = View;
            types = ViewService.GetTypeMapping()
                .Select(x => x.Key)
                .ToHashSet();
            return Task.CompletedTask;
        }

        protected Task SubmitAsync(LineChart arg)
        {
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }

        private void OnChange(object value)
        {
            var row = value as Row;
            Item.RowId = row.Id;
            Item.Row = row;
        }
    }
}
