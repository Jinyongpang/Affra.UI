using AntDesign;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class GasDefermentDetailManagement
    {
        private const string All = "All";
        private Menu menu;
        private GasDefermentDetailGrid gasDefermentDetailGrid { get; set; }
        private bool isCollapsed = false;
        private Task OnGasMenuItemSelectAsync(MenuItem menuItem)
        {
            gasDefermentDetailGrid.GasDefermentDetailFilter = menuItem.Key;
            return gasDefermentDetailGrid.ReloadAsync();
        }
    }
}
