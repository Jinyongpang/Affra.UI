using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using Microsoft.OData.Client;

namespace UserODataService.Affra.Service.User.Domain.Users
{
    public partial class User
    {
        private UserPersonalization _userPersonalization;

        [IgnoreClientProperty]
        public UserPersonalization UserPersonalization
        {
            get
            {
                this._userPersonalization = this.Personalization is null
                    ? new ()
                    : JsonSerializer.Deserialize<UserPersonalization>(this.Personalization);
                return this._userPersonalization;
            }
            set 
            { 
                this.Personalization = JsonSerializer.Serialize(this._userPersonalization); 
            }
        }
    }
}
