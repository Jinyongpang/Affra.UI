using System.Security.Claims;
using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using Microsoft.OData.Client;
using UserODataService.Affra.Service.User.Domain.Roles;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Users
{
    public class UserService : IUserService
    {
        private const string Administrator = "Administrator";
        private const string UsernameKey = "preferred_username";
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly IGlobalDataSource globalDataSource;

        public UserService(IUserUnitOfWork userUnitOfWork, IGlobalDataSource globalDataSource)
        {
            this._userUnitOfWork = userUnitOfWork;
            this.globalDataSource = globalDataSource;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            var email = this.GetEmail(user).ToLower();
            var query = (DataServiceQuery<User>)this._userUnitOfWork.UserRepository.Get();
            var userFromService = (await ((DataServiceQuery<User>) query
                .Expand(x => x.RoleGroup)
                .Where(x => x.Username == email))
                .ExecuteAsync())
                .ToList()
                .FirstOrDefault();

            return userFromService;

        }

        public string GetEmail(ClaimsPrincipal user)
        {
            return this.GetValue(user, UsernameKey);
        }

        public string GetValue(ClaimsPrincipal user, string key)
        {
            return user.Claims
                .Where(x => x.Type.Contains(UsernameKey))
                .FirstOrDefault()?
                .Value;
        }

        public string GetAvatarName(string name)
        {
            string[] names = name.Split(' ');
            string result = string.Empty;
            result += names[0][0];
            if (names.Length > 1)
            {
                result += names[1][0];
            }

            return result;
        }

        public UserPersonalization GetUserPersonalization(User user)
        {
            return JsonSerializer.Deserialize<UserPersonalization>(user.Personalization);
        }

        public async Task<ICollection<Role>> GetRolesAsync()
        {
            var query = (DataServiceQuery<Role>)this._userUnitOfWork.RoleRepository.Get()
                .OrderBy(x => x.Name);
            var roles = (await query.ExecuteAsync())
                .ToList();

            return roles;
        }
        public async Task<Role> GetRoleAsync(string name)
        {
            var query = (DataServiceQuery<Role>)this._userUnitOfWork.RoleRepository.Get()
                .Where(x => x.Name.ToUpper() == name.ToUpper());
            var role = (await query.ExecuteAsync())
                .FirstOrDefault();

            return role;

        }

        public async Task<bool> CheckHasPermissionAsync(ClaimsPrincipal claimsPrincipal, Permission permission)
        {
            Console.WriteLine($"User is null = {globalDataSource.User is null}");
            Console.WriteLine($"Claism principal is null = {claimsPrincipal is not null}");
            Console.WriteLine($"claimsPrincipal.Identity?.IsAuthenticated = {claimsPrincipal?.Identity?.IsAuthenticated}");
            if (globalDataSource.User is null
                && claimsPrincipal is not null
                && claimsPrincipal.Identity?.IsAuthenticated == true)
            {
                this.globalDataSource.User = await this.GetUserAsync(claimsPrincipal);
            }
            if (this.globalDataSource.User is null)
            {
                Console.WriteLine("User is null.");
                return false;
            }
            else if (this.globalDataSource.User.Email.Equals("jianyi.lim@affra.onmicrosoft.com", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(globalDataSource.User?.Role))
            {
                Console.WriteLine("Empty role.");
                return false;
            }
            else if (permission is null)
            {

                Console.WriteLine("No Permission.");
                return true;
            }
            else if (globalDataSource.User.Role == Administrator)
            {
                Console.WriteLine("Is administrator.");
                return true;
            }
            else if (this.globalDataSource.User.RoleGroup?.Permissions?
                .Where(x => x.Name.Equals(permission.Name, StringComparison.OrdinalIgnoreCase))
                .Where(x => !permission.HasWritePermission
                    || x.HasWritePermission)
                .Where(x => !permission.HasReadPermissoin
                    || x.HasReadPermissoin)
                .Any() == true)
            {
                return true;
            }

            Console.WriteLine("False result.");
            return false;
        }
    }
}
