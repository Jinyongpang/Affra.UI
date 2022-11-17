using AntDesign;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class OilDefermentDetailManagement
    {
        private const string All = "All";
        private Menu menu;
        private OilDefermentDetailGrid oilDefermentDetailGrid { get; set; }
        private bool isCollapsed = false;
        private Task OnOilMenuItemSelectAsync(MenuItem menuItem)
        {
            oilDefermentDetailGrid.OilDefermentDetailFilter = menuItem.Key;
            return oilDefermentDetailGrid.ReloadAsync();
        }
    }
}
