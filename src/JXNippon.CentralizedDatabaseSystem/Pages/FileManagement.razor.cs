using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.FileManagement;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class FileManagement
    {
        private FileManagementFilterPanel fileManagementFilterPanel;
        private FileManagementDataList fileManagementDataList;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            fileManagementDataList.FileManagementFilter = fileManagementFilterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await fileManagementDataList.ReloadAsync();
        }
    }
}
