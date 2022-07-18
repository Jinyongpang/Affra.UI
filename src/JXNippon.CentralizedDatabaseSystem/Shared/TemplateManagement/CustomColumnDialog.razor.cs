using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.TemplateManagement
{
    public partial class CustomColumnDialog
    {
        [Parameter] public CustomColumn Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<string> types = 
            new []
            { 
                "string",
                "bool",
                "integer",
                "decimal",
                "date",
                "datetime",
            };

        private bool isViewing { get => MenuAction == 3; }

        protected override Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

        protected Task SubmitAsync(CustomColumn arg)
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
