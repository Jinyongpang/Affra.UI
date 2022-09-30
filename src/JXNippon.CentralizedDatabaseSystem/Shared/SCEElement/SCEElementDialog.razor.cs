using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.SCEElements;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.SCEElement
{
    public partial class SCEElementDialog
    {
        [Parameter] public SCEElementRecord Item { get; set; }
        [Parameter] public IEnumerable<SCEElementGroupRecord> GroupItem { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private long ddSelectedValue = 0;

        private bool isViewing { get => MenuAction == 3; }
        protected override void OnInitialized()
        {
            if (Item.SCEGroupId != null)
            {
                ddSelectedValue = (long)Item.SCEGroupId;
            }
        }
        protected Task SubmitAsync(SCEElementRecord arg)
        {
            arg.SCEGroupId = ddSelectedValue;
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
