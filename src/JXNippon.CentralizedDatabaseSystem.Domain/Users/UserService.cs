using System.Security.Claims;
using System.Text.Json;
using Microsoft.OData.Client;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Users
{
    public class UserService : IUserService
    {
        private const string UsernameKey = "preferred_username";
        private readonly IUserUnitOfWork _userUnitOfWork;

        public UserService(IUserUnitOfWork userUnitOfWork)
        {
            this._userUnitOfWork = userUnitOfWork;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            var email = this.GetEmail(user).ToLower();
            var userFromService = (await ((DataServiceQuery<User>)this._userUnitOfWork.UserRepository.Get()
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
    }
}
