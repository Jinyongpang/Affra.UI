using AntDesign;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class ManagementOfChange
    {
        private const string All = "All";
        private ManagementOfChangeDataList managementOfChangeDataList;
        private Menu menu;
        private string search;

        private Task ReloadAsync(string status = null)
        {
            managementOfChangeDataList.ManagementOfChangeFilter.Status = GetStatusFilter(status);
            managementOfChangeDataList.ManagementOfChangeFilter.Search = search;
            return managementOfChangeDataList.ReloadAsync();
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