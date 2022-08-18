using Affra.Core.Domain.UnitOfWorks;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Users
{
    public interface IUserUnitOfWork : IUnitOfWork
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<UserActivity> UserActivityRepository { get; }
    }
}