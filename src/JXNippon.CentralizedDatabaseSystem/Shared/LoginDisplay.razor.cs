using System.Security.Claims;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
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

        public Task ReloadAsync()
        {
            this.StateHasChanged();
            return Task.CompletedTask;
        }

        private async Task BeginLogout(MouseEventArgs args)
        {
            await this.LogUserActivityAsync(ActivityType.Logout);
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
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
                    var service = this.GetGenericService<User>(serviceScope);

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
