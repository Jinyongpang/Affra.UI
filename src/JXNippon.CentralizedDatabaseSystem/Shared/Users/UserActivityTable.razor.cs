using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using AntDesign.TableModels;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class UserActivityTable
    {
        private bool _loading = false;
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private int _total = 0;
        private UserActivity[] _data = Array.Empty<UserActivity>();

        [Parameter]
        public User User { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        public CommonFilter CommonFilter { get; set; }

        private async Task HandleTableChangeAsync(QueryModel<UserActivity> queryModel)
        {
            _loading = true;

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<UserActivity>? userService = this.GetGenericService(serviceScope);
            var query = queryModel.ExecuteQuery(userService.Get());

            Microsoft.OData.Client.QueryOperationResponse<UserActivity>? response = await query
                .Where(x => x.Username.ToUpper() == User.Username.ToUpper())
                .Skip(this._pageSize * (this._pageIndex - 1))
                .Take(this._pageSize)
                .ToQueryOperationResponseAsync<UserActivity>();

            _total = (int)response.Count; 
            _loading = false;
            _data = response.ToArray();
            
        }
        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<UserActivity> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<UserActivity, IUserUnitOfWork>>();
        }
    }
}
