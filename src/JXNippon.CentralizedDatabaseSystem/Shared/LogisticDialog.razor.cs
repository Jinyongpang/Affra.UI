using Affra.Core.Domain.Services;
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

        protected async Task SubmitAsync(DailyLogistic arg)
        {
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(serviceScope);

                if (arg.Id > 0)
                {
                    await service.UpdateAsync(arg, arg.Id);
                    NotificationService.Notify(new()
                    {
                        Summary = "Updated successfully.",
                        Detail = "",
                        Severity = NotificationSeverity.Success,
                        Duration = 10000,
                    });
                }
                else
                {
                    arg.Date = arg.Date.ToUniversalTime();
                    await service.InsertAsync(arg);
                    NotificationService.Notify(new()
                    {
                        Summary = "Added new logistic item.",
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
