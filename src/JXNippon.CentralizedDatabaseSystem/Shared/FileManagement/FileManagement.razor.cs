using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Workspaces;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace JXNippon.CentralizedDatabaseSystem.Shared.FileManagement
{
    public class Folder
    {
        public string FolderName { get; set; }
    }
    public partial class FileManagement
    {
        private const string All = "All";
        private static Folder[] Folders = new string[] 
        { 
            "FPSODailyReport", 
            "FPSOLabDailyReport", 
            "ProcessDailyReport", 
            "InstrumentDailyReport", 
            "PIMSAndMETReport",
            "DailyGasProductionDeliverySchedule",
            "PEMonthlyReport",
            "VendorReport",
            "DefermentDetail",
            "WellTestReport",
            "FPSOProductionDBReport",
            "CondensateStreamReport",
            "DailyGasMeteringReport",
            "Templates",
            "AvailabilityAndReliabilityReport",
        }
            .OrderBy(x => x)
            .Select(x => new Folder()
            { 
                FolderName = x,
            })
            .ToArray();

        private FileManagementDataList fileManagementDataList;
        private Menu menu;
        private string search;
        private string folder;

        [Inject] private IWorkspaceService WorkspaceService { get; set; }

        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        private Task ReloadAsync(string status = null, string searchInput = null)
        {
            fileManagementDataList.FileManagementFilter.Status = GetStatusFilter(status);
            fileManagementDataList.FileManagementFilter.Search = searchInput ?? search;
            fileManagementDataList.Folder = this.folder;

            return fileManagementDataList.ReloadAsync();
        }

        private string GetStatusFilter(string status = null)
        {
            status ??= menu.SelectedKeys.FirstOrDefault();
            if (status != All)
            {
                return status;
            }

            return null;
        }

        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            return this.ReloadAsync(menuItem.Key);
        }

        private async Task UploadAsync(InputFileChangeEventArgs e)
        {
            try
            {
                foreach (var file in e.GetMultipleFiles(30))
                {
                    await this.WorkspaceService.UploadAsync(this.folder, new Domain.WorkspaceAPI.FileParameter(file.OpenReadStream(512000000), file.Name));
                }
                this.AffraNotificationService.NotifySuccess("File uploaded!");
            }
            catch (Exception ex)
            { 
                this.AffraNotificationService.NotifyException(ex);
            }
        }
    }
}
