using Microsoft.AspNetCore.Authorization;

namespace JXNippon.CentralizedDatabaseSystem.Handlers
{
    public class UserRoleAuthorizePermission : IAuthorizationRequirement
    {
        public UserRoleAuthorizePermission(PageSection pageSection) => PageSection = pageSection;

        public PageSection PageSection { get; set; }
    }
}
