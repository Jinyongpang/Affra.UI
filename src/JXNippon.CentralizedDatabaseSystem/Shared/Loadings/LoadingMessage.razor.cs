using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Loadings
{
    public partial class LoadingMessage
    {
        [Parameter] public string Message { get; set; }

        [Parameter ] public Task Task { get; set; }

        [Inject] public DialogService DialogService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task;
            await base.OnInitializedAsync();
            this.DialogService.Close();
        }
    }
}
