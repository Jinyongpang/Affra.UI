using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;
using WellProductionCalculations = CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations;

namespace JXNippon.CentralizedDatabaseSystem.Shared.WellCoefficient
{
    public partial class WellCoefficientDialog
    {
        [Parameter] public WellProductionCalculations.WellCoefficient Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }

        private bool isViewing { get => MenuAction == 3; }
        protected Task SubmitAsync(WellProductionCalculations.WellCoefficient arg)
        {
            arg.LastUpdatedDateUI = DateTime.Now;
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
