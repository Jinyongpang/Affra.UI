using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign.TableModels;
using CentralizedDatabaseSystemODataService.Affra.Core.Domain.AuditTrails;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AuditTrails
{
    public partial class AuditTrailTable
    {
        private bool _loading = false;
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private int _total = 0;
        private AuditTrail[] _data = Array.Empty<AuditTrail>();

        [Parameter] public long? Id { get; set; }
        [Parameter] public CentralizedDatabaseSystemODataService.Affra.Core.Domain.AuditTrails.Action? Action { get; set; }
        [Parameter] public string TableName { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        protected override Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

        private async Task HandleTableChangeAsync(QueryModel<AuditTrail> queryModel)
        {
            _loading = true;

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<AuditTrail>? userService = this.GetGenericService(serviceScope);
            var query = queryModel.ExecuteQuery(userService.Get());

            if (this.Id is not null)
            {
                query = query.Where(x => x.OwnerId == this.Id);
            }

            if (this.Action is not null)
            {
                query = query.Where(x => x.Action == this.Action);
            }

            Microsoft.OData.Client.QueryOperationResponse<AuditTrail>? response = await query
                .Where(x => x.TableName == this.TableName)
                .Skip(this._pageSize * (this._pageIndex - 1))
                .Take(this._pageSize)
                .ToQueryOperationResponseAsync<AuditTrail>();

            _total = (int)response.Count;
            _loading = false;
            _data = response.ToArray();
        }

        private IGenericService<AuditTrail> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<AuditTrail, ICentralizedDatabaseSystemUnitOfWork>>();
        }
    }
}
