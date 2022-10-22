using System.Security.Claims;
using UserODataService.Affra.Service.User.Domain.Roles;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Users
{
    public interface IUserService
    {
        Task<bool> CheckHasPermissionAsync(ClaimsPrincipal claimsPrincipal, Permission permission);
        string GetAvatarName(string name);
        string GetEmail(ClaimsPrincipal user);
        Task<Role> GetRoleAsync(string name);
        Task<ICollection<Role>> GetRolesAsync();
        Task<User> GetUserAsync(ClaimsPrincipal user);
        UserPersonalization GetUserPersonalization(User user);
        string GetValue(ClaimsPrincipal user, string key);
    }
}