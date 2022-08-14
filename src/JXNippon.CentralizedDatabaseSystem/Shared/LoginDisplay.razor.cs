using System.Security.Claims;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class LoginDisplay
    {

        [Inject]
        private IServiceProvider _serviceProvider { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private AffraNotificationService _affraNotificationService { get; set; }

        private async Task BeginLogout(MouseEventArgs args)
        {
            await this.LogUserActivityAsync(ActivityType.Logout);
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }

        private string GetAvatarName(string name)
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

        private string GetEmail(ClaimsPrincipal user)
        {
            return this.GetValue(user, "preferred_username");
        }

        private string GetValue(ClaimsPrincipal user, string key)
        {
            return user.Claims
                .Where(x => x.Type.Contains(key))
                .FirstOrDefault()?
                .Value;
        }

        private async Task LogUserActivityAsync(ActivityType activityType, ClaimsPrincipal? user = null)
        {
            try
            {
                using var serviceScope = this._serviceProvider.CreateScope();
                var service = this.GetGenericService<UserActivity>(serviceScope);
                await service.InsertAsync(new UserActivity()
                {
                    Username = user?.Identity?.Name,
                    ActivityType = activityType,
                    Site = this._navigationManager.BaseUri.ToString(),
                });
            }
            catch (Exception ex)
            {
                this._affraNotificationService.NotifyException(ex);
            }
        }

        private IGenericService<T> GetGenericService<T>(IServiceScope serviceScope) where T : class
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<T, IUserUnitOfWork>>();
        }
    }
}
