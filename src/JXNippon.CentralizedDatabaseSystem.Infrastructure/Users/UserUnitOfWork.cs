using Affra.Core.Domain.UnitOfWorks;
using Affra.Core.Infrastructure.OData.UnitOfWorks;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;
using UserODataService.Affra.Service.User.Domain.Users;
using UserODataService.Default;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Users
{
    public class UserUnitOfWork : ODataUnitOfWorkBase, IUserUnitOfWork
    {
        public UserUnitOfWork(IODataClientFactory oDataClientFactory, IOptions<UserConfigurations> userConfigurations)
            : base(oDataClientFactory.CreateClient<Container>(new Uri(userConfigurations.Value.Url), nameof(UserUnitOfWork)))
        {
        }

        public IGenericRepository<User> UserRepository => this.GetGenericRepository<User>();
        public IGenericRepository<UserActivity> UserActivityRepository => this.GetGenericRepository<UserActivity>();
    }
}