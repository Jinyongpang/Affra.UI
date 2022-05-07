using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MajorEquipmentStatus;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class MajorEquipmentStatus
    {
        private RadzenDataGrid<DailyMajorEquipmentStatus> grid;
        private IEnumerable<DailyMajorEquipmentStatus> dailyMajorEquipmentStatuses;
        private DailyMajorEquipmentStatus dailyMajorEquipmentStatusToInsert;
        private int count;
        private bool isLoading;

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
        private async Task Reload()
        {
            grid.Reset(true);
            await grid.Reload();
        }
        private async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;
            using var serviceScope = ServiceProvider.CreateScope();
            var dailyMajorEquipmentStatusService = this.GetGenericService(serviceScope);
            var dailyMajorEquipmentStatusResponse = await dailyMajorEquipmentStatusService.Get()
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DailyMajorEquipmentStatus>();

            count = (int)dailyMajorEquipmentStatusResponse.Count;
            dailyMajorEquipmentStatuses = dailyMajorEquipmentStatusResponse.ToList();

            isLoading = false;
        }
        private async Task EditRow(DailyMajorEquipmentStatus dailyMajorEquipmentStatus)
        {
            await grid.EditRow(dailyMajorEquipmentStatus);
        }
        private async Task SaveRow(DailyMajorEquipmentStatus dailyMajorEquipmentStatus)
        {
            try
            {
                if (dailyMajorEquipmentStatus == dailyMajorEquipmentStatusToInsert)
                {
                    dailyMajorEquipmentStatusToInsert = null;
                }
                using var serviceScope = ServiceProvider.CreateScope();
                var dailyMajorEquipmentStatusService = this.GetGenericService(serviceScope);
                await dailyMajorEquipmentStatusService.UpdateAsync(dailyMajorEquipmentStatus, dailyMajorEquipmentStatus.Id);
                await grid.UpdateRow(dailyMajorEquipmentStatus);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
        private void CancelEdit(DailyMajorEquipmentStatus dailyMajorEquipmentStatus)
        {
            if (dailyMajorEquipmentStatus == dailyMajorEquipmentStatusToInsert)
            {
                dailyMajorEquipmentStatusToInsert = null;
            }

            grid.CancelEditRow(dailyMajorEquipmentStatus);

        }
        private async Task DeleteRow(DailyMajorEquipmentStatus dailyMajorEquipmentStatus)
        {
            try
            {
                if (dailyMajorEquipmentStatus == dailyMajorEquipmentStatusToInsert)
                {
                    dailyMajorEquipmentStatusToInsert = null;
                }

                if (dailyMajorEquipmentStatuses.Contains(dailyMajorEquipmentStatus))
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var dailyMajorEquipmentStatusService = this.GetGenericService(serviceScope);
                    await dailyMajorEquipmentStatusService.DeleteAsync(dailyMajorEquipmentStatus);
                    await grid.Reload();
                }
                else
                {
                    grid.CancelEditRow(dailyMajorEquipmentStatus);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
        private void HandleException(Exception ex)
        {
            NotificationService.Notify(new()
            {
                Summary = "Error",
                Detail = ex.InnerException?.ToString(),
                Severity = NotificationSeverity.Error,
                Duration = 120000,
            });
        }
        private IGenericService<DailyMajorEquipmentStatus> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyMajorEquipmentStatus, ICentralizedDatabaseSystemUnitOfWork>>();
        }
    }
}
