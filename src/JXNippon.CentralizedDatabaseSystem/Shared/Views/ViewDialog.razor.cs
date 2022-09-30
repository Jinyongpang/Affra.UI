using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewDialog
    {
        private string[] datas = new[] { "Operation", "WellAllocation", "Deferment", "PEReport", };
        [Parameter] public View Item { get; set; }

        [Inject] private DialogService DialogService { get; set; }
 

        protected Task SubmitAsync(View arg)
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
