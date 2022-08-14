using System.Security.Claims;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Authentication
    {
        [Parameter]
        public string Action { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationState { get; set; }

        [Inject]
        private IServiceProvider _serviceProvider { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private AffraNotificationService _affraNotificationService { get; set; }

        [Inject]
        private IGlobalDataSource _globalDataSource { get; set; }

        private bool _loggedIn = false;

        public async void OnLogInSucceededAsync()
        {
            if (this._loggedIn)
            {
                return;
            }
            this._loggedIn = true;
            ClaimsPrincipal? user =
                (await AuthenticationState).User;

            if (user.Identity.IsAuthenticated)
            {
                await this.LogUserActivityAsync(ActivityType.Login, user);
                using var serviceScope = this._serviceProvider.CreateScope();
                var service = this.GetGenericService<User>(serviceScope);
                var email = this.GetEmail(user).ToLower();
                var userFromService = (await service.Get()
                    .Where(x => x.Username == email)
                    .ToQueryOperationResponseAsync<User>())
                    .ToList()
                    .FirstOrDefault();
                this._globalDataSource.User = userFromService;
            }
        }

        public void OnLogOutSucceededAsync(RemoteAuthenticationState remoteAutenticationState)
        {
            this._loggedIn = false;
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
