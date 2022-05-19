using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Logistics;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class LogisticDialog
    {
        [Parameter] public DailyLogistic Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
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
                    NotificationService.Notify(new()
                    {
                        Summary = "Item updated.",
                        Detail = "",
                        Severity = NotificationSeverity.Success,
                        Duration = 10000,
                    });
                }
                else
                {                 
                    await service.InsertAsync(arg);
                    NotificationService.Notify(new()
                    {
                        Summary = "New item added.",
                        Detail = "",
                        Severity = NotificationSeverity.Success,
                        Duration = 10000,
                    });
                }


                DialogService.Close(true);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new()
                {
                    Summary = "Error",
                    Detail = ex.InnerException?.ToString(),
                    Severity = NotificationSeverity.Error,
                    Duration = 120000,
                });
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
