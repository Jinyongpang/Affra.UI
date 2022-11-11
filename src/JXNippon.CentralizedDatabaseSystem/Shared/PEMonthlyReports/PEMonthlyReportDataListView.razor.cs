using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;

namespace JXNippon.CentralizedDatabaseSystem.Shared.PEMonthlyReports
{
    public partial class PEMonthlyReportDataListView
    {
        private const string All = "All";
        private PEMonthlyReportDataList peMonthlyReportDataList;
        private Menu menu;
        private string search;
        private bool isCollapsed = false;
        private IDateFilterComponent dateFilterComponent;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private async Task ReloadAsync(string status = null)
        {
            peMonthlyReportDataList.Filter.Status = GetStatusFilter(status);
            peMonthlyReportDataList.Filter.Search = search;
            peMonthlyReportDataList.Filter.DateRange = new DateRange { Start = dateFilterComponent?.Start, End = dateFilterComponent?.End };
            await peMonthlyReportDataList.ReloadAsync();
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
