using Microsoft.AspNetCore.Authorization;
using UserODataService.Affra.Service.User.Domain.Roles;

namespace JXNippon.CentralizedDatabaseSystem.Handlers
{
    public class UserRoleAuthorizePermission : IAuthorizationRequirement
    {
        public UserRoleAuthorizePermission(Permission permission) => Permission = permission;

        public Permission Permission { get; set; }
    }
}
