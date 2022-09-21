using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Users
{
    public class UserServiceClient : OpenAPI.UserService.UserServiceClient
    {
        public UserServiceClient(IOptions<UserConfigurations> userConfigurations, HttpClient httpClient) : base(httpClient)
        {
            this.BaseUrl = userConfigurations.Value.OpenAPIUrl;
        }
    }
}
