using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class GridColumnStyleDialog
    {
        [Parameter] public ConditionalStyling Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<string> properties;

        private bool isViewing { get => MenuAction == 3; }

        private static IEnumerable<string> ConditionalStylingOperators = Enum.GetValues(typeof(ConditionalStylingOperator))
            .Cast<ConditionalStylingOperator>()
            .Select(x => x.ToString())
            .ToList();

        protected override Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

        protected Task SubmitAsync(ConditionalStyling arg)
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
