using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class ProducedWaterTreatmentSystemManagement
    {
        private RadzenDataGrid<DailyProducedWaterTreatmentSystem> grid;
        private IEnumerable<DailyProducedWaterTreatmentSystem> dailyProducedWaterTreatmentSystems;
        private DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystemToInsert;
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
            var dailyProducedWaterTreatmentSystemService = this.GetGenericService(serviceScope);
            var dailyProducedWaterTreatmentSystemsResponse = await dailyProducedWaterTreatmentSystemService.Get()
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DailyProducedWaterTreatmentSystem>();

            count = (int)dailyProducedWaterTreatmentSystemsResponse.Count;
            dailyProducedWaterTreatmentSystems = dailyProducedWaterTreatmentSystemsResponse.ToList();

            isLoading = false;
        }

        private async Task EditRow(DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystem)
        {
            await grid.EditRow(dailyProducedWaterTreatmentSystem);
        }

        private async Task SaveRow(DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystem)
        {
            try
            {
                if (dailyProducedWaterTreatmentSystem == dailyProducedWaterTreatmentSystemToInsert)
                {
                    dailyProducedWaterTreatmentSystemToInsert = null;
                }
                using var serviceScope = ServiceProvider.CreateScope();
                var dailyProducedWaterTreatmentSystemService = this.GetGenericService(serviceScope);
                await dailyProducedWaterTreatmentSystemService.UpdateAsync(dailyProducedWaterTreatmentSystem, dailyProducedWaterTreatmentSystem.Id);
                await grid.UpdateRow(dailyProducedWaterTreatmentSystem);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        private void CancelEdit(DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystem)
        {
            if (dailyProducedWaterTreatmentSystem == dailyProducedWaterTreatmentSystemToInsert)
            {
                dailyProducedWaterTreatmentSystemToInsert = null;
            }

            grid.CancelEditRow(dailyProducedWaterTreatmentSystem);

        }

        private async Task DeleteRow(DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystem)
        {
            try
            {
                if (dailyProducedWaterTreatmentSystem == dailyProducedWaterTreatmentSystemToInsert)
                {
                    dailyProducedWaterTreatmentSystemToInsert = null;
                }

                if (dailyProducedWaterTreatmentSystems.Contains(dailyProducedWaterTreatmentSystem))
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var dailyProducedWaterTreatmentSystemService = this.GetGenericService(serviceScope);
                    await dailyProducedWaterTreatmentSystemService.DeleteAsync(dailyProducedWaterTreatmentSystem);
                    await grid.Reload();
                }
                else
                {
                    grid.CancelEditRow(dailyProducedWaterTreatmentSystem);
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

        private IGenericService<DailyProducedWaterTreatmentSystem> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyProducedWaterTreatmentSystem, ICentralizedDatabaseSystemUnitOfWork>>();
        }
    }
}
