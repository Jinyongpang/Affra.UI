using Microsoft.AspNetCore.Components;
using Radzen;
using MajorEquipmentStatuses = CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MajorEquipmentStatuses;

namespace JXNippon.CentralizedDatabaseSystem.Shared.MajorEquipment
{
    public partial class MajorEquipmentStatusDialog
    {
        [Parameter] public MajorEquipmentStatuses.DailyMajorEquipmentStatus Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected Task SubmitAsync(MajorEquipmentStatuses.DailyMajorEquipmentStatus arg)
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
