using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class RoleDialog
    {
        [Parameter] public Role Item { get; set; }

        [Inject] private DialogService DialogService { get; set; }


        protected Task SubmitAsync(Role arg)
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
