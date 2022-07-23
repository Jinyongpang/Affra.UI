using Microsoft.AspNetCore.Components;
using Radzen;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class ManagementOfChangeCardTemplate
    {
        [Parameter] public string RecordCode { get; set; }
        [Parameter] public string Description { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        private async Task ShowDialogAsync(string recordCode, string description, string title)
        {
            await DialogService.OpenAsync<MOCFormTemplate>(title,
                    new Dictionary<string, object>() { { "RecordCode", recordCode }, { "Description", description } },
                    Constant.MOHDialogOptions);
        }
    }
}
