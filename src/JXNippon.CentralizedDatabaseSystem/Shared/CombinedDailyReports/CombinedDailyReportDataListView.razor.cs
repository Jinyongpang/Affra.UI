using AntDesign;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportDataListView
    {
        private const string All = "All";
        private CombinedDailyReportDataList combinedDailyReportDataList;
        private Menu menu;
        private string search;

        private async Task ReloadAsync(string status = null)
        {
            combinedDailyReportDataList.Filter.Status = GetStatusFilter(status);
            combinedDailyReportDataList.Filter.Search = search;
            await combinedDailyReportDataList.ReloadAsync();
            this.StateHasChanged();
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
