using System.Security.Claims;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
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

        [Inject]
        private IUserService _userService { get; set; }

        [Inject] private DialogService DialogService { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }

        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public Task ReloadAsync()
        {
            StateHasChanged();
            return Task.CompletedTask;
        }

        protected override async Task OnInitializedAsync()
        {
            var authstate = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            if (authstate?.User?.Identity?.IsAuthenticated == true)
            {
                await RefreshUserAsync(authstate.User);
            }
        }

        private async Task BeginLogout(MouseEventArgs args)
        {
            await LogUserActivityAsync(ActivityType.Logout);
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }

        private async Task LogUserActivityAsync(ActivityType activityType, ClaimsPrincipal? user = null)
        {
            try
            {
                using var serviceScope = _serviceProvider.CreateScope();
                var service = GetGenericService<UserActivity>(serviceScope);
                await service.InsertAsync(new UserActivity()
                {
                    Username = user?.Identity?.Name,
                    ActivityType = activityType,
                    Site = _navigationManager.BaseUri.ToString(),
                });
            }
            catch (Exception ex)
            {
                _affraNotificationService.NotifyException(ex);
            }
        }

        private async Task RefreshUserAsync(ClaimsPrincipal? user)
        {

            if (user.Identity.IsAuthenticated)
            {
                using var serviceScope = _serviceProvider.CreateScope();
                var service = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
                var userFromService = await service.GetUserAsync(user);
                GlobalDataSource.User = userFromService;
            }
        }


        private async Task ShowDialogAsync()
        {
            User data = GlobalDataSource.User;
            dynamic? response;
            response = await DialogService.OpenAsync<UserDialog>("Edit",
                       new Dictionary<string, object>() { { "Item", data }, { "MenuAction", 1 }, { "IsUserEdit", true } },
                       Constant.DialogOptions);

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = GetGenericService<User>(serviceScope);

                    if (data.Id != Guid.Empty)
                    {
                        await service.UpdateAsync(data, data.Id);
                        _affraNotificationService.NotifyItemUpdated();
                    }
                    else
                    {
                        await service.InsertAsync(data);
                        _affraNotificationService.NotifyItemCreated();
                    }
                    GlobalDataSource.User = data;

                }
                catch (Exception ex)
                {
                    _affraNotificationService.NotifyException(ex);
                }
            }

        }

        private IGenericService<T> GetGenericService<T>(IServiceScope serviceScope) where T : class
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<T, IUserUnitOfWork>>();
        }
    }
}
