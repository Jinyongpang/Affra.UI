using AntDesign;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class UserManagement
    {
        private const string All = "All";
        private UserDataList userDataList;
        private Menu menu;
        private string search;

        private Task ReloadAsync(string status = null)
        {
            userDataList.CommonFilter.Search = search;
            return userDataList.ReloadAsync();
        }
    }
}
