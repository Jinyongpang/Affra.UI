using AntDesign;

namespace JXNippon.CentralizedDatabaseSystem.Shared.FileManagement
{
    public partial class FileManagement
    {
        private const string All = "All";
        private FileManagementDataList fileManagementDataList;
        private Menu menu;
        private string search;

        private Task ReloadAsync(string status = null)
        {
            fileManagementDataList.FileManagementFilter.Status = GetStatusFilter(status);
            fileManagementDataList.FileManagementFilter.Search = search;
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
    }
}
