using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using Microsoft.AspNetCore.Authorization;

namespace JXNippon.CentralizedDatabaseSystem.Handlers
{
    public class UserRoleAuthorizeHandler : AuthorizationHandler<UserRoleAuthorizePermission>
    {
        private readonly IServiceProvider serviceProvider;

        public UserRoleAuthorizeHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleAuthorizePermission requirement)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var service = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
            var result = await service.CheckHasPermissionAsync(context.User, requirement.Permission);
            if (result)
            {
                context.Succeed(requirement);
            }
        }
    }
}
