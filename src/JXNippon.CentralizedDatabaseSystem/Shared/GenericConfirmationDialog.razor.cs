using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class GenericConfirmationDialog
    {
        [Inject] private DialogService DialogService { get; set; }
        private void ConfirmClicked() => DialogService.Close(true);
        private void CancelClicked() => DialogService.Close(false);
    }
}
