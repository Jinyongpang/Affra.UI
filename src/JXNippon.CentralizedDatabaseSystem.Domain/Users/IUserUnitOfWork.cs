using Affra.Core.Domain.UnitOfWorks;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Users
{
    public interface IUserUnitOfWork : IUnitOfWork
    {
        IGenericRepository<User> ViewRepository { get; }
        IGenericRepository<UserActivity> RowRepository { get; }
    }
}