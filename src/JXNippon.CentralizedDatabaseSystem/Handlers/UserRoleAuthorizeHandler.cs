using JXNippon.CentralizedDatabaseSystem.Configurations;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace JXNippon.CentralizedDatabaseSystem.Handlers
{
    public class UserRoleAuthorizeHandler : AuthorizationHandler<UserRoleAuthorizePermission>
    {
        private const string Administrator = "Administrator";
        private readonly IGlobalDataSource globalDataSource;
        private readonly RoleAuthorizationConfigurations roleAuthorizationConfigurations;
        private readonly IServiceProvider serviceProvider;

        public UserRoleAuthorizeHandler(IGlobalDataSource globalDataSource, IOptions<RoleAuthorizationConfigurations> roleAuthorizationConfigurations, IServiceProvider serviceProvider)
        {
            this.globalDataSource = globalDataSource;
            this.roleAuthorizationConfigurations = roleAuthorizationConfigurations.Value;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleAuthorizePermission requirement)
        {
            if (globalDataSource.User is null
                && context.User is not null
                && context.User.Identity?.IsAuthenticated == true)
            {
                using var serviceScope = this.serviceProvider.CreateScope();
                var service = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
                var userFromService = await service.GetUserAsync(context.User);
                this.globalDataSource.User = userFromService;
            }

            if (string.IsNullOrEmpty(globalDataSource.User?.Role))
            {
                return;
            }

            if (requirement.PageSection == PageSection.Undefined)
            {
                context.Succeed(requirement);
            }
            else if (globalDataSource.User.Role == Administrator || this.globalDataSource.User.Email.Equals("jianyi.lim@affra.onmicrosoft.com", StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }
            else if (!roleAuthorizationConfigurations.RolePageSections.TryGetValue(globalDataSource.User.Role, out var authorizationPages))
            {
                return;
            }
            else
            {
                if (authorizationPages.ToList().Contains(requirement.PageSection))
                {
                    context.Succeed(requirement);
                }
            }

            return;
        }
    }
}
