using System.Security.Claims;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Users
{
    public interface IUserService
    {
        string GetAvatarName(string name);
        string GetEmail(ClaimsPrincipal user);
        Task<User> GetUserAsync(ClaimsPrincipal user);
        UserPersonalization GetUserPersonalization(User user);
        string GetValue(ClaimsPrincipal user, string key);
    }
}