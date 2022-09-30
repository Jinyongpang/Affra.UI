using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class DefermentDetailManagement
    {
        private const string All = "All";
        private Menu menu;
        private DefermentDetailGrid defermentDetailGrid { get; set; }
        private readonly List<string> defermentDetailStatusList = new List<string>();

        protected override Task OnInitializedAsync()
        {
            return LoadDefermentStatusAsync();
        }
        private Task LoadDefermentStatusAsync()
        {
            foreach (var val in Enum.GetValues(typeof(DefermentDetailStatus)).Cast<DefermentDetailStatus>())
            {
                defermentDetailStatusList.Add(val.ToString());
            }

            StateHasChanged();

            return Task.CompletedTask;
        }
        private Task InitDefermentDetailAsync()
        {
            if (defermentDetailGrid.DefermentDetailFilter == null)
            {
                defermentDetailGrid.DefermentDetailFilter = All;
            }

            return defermentDetailGrid.ReloadAsync();
        }
        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            defermentDetailGrid.DefermentDetailFilter = menuItem.Key;
            return defermentDetailGrid.ReloadAsync();
        }
    }
}
