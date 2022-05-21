using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Logistics;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class LogisticDialog
    {
        [Parameter] public DailyLogistic Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<Logistic> datas;

        protected override async Task OnInitializedAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            datas = (await serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<Logistic, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<Logistic>()).ToList();
        }
        protected async Task SubmitAsync(DailyLogistic arg)
        {
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(serviceScope);
                arg.Date = arg.Date.ToUniversalTime();

                if (arg.Id > 0)
                {
                    await service.UpdateAsync(arg, arg.Id);
                    AffraNotificationService.NotifyItemUpdated();
                }
                else
                {
                    await service.InsertAsync(arg);
                    AffraNotificationService.NotifyItemCreated();
                }
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
            finally
            {
                DialogService.Close(true);
            }
        }
        private IGenericService<DailyLogistic> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyLogistic, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
