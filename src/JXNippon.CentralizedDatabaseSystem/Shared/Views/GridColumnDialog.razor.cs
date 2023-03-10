using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class GridColumnDialog
    {
        [Parameter] public GridColumn Item { get; set; }
        [Parameter] public Grid Grid { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<string> Types { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<string> properties;

        private bool isViewing { get => MenuAction == 3; }

        protected override Task OnInitializedAsync()
        {
            this.RefreshTypeProperties();
            return Task.CompletedTask;
        }

        protected Task SubmitAsync(GridColumn arg)
        {
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }

        private void RefreshTypeProperties()
        {
            Type type = this.Item.ActualType ?? Grid.ActualType;

            properties = type.GetProperties()
                .Where(p => p.Name != "Date")
                .Select(prop => prop.Name)
                .ToList();
        }
    }
}
