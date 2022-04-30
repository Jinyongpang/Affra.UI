using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PowerGenerationAndDistributions;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class PowerGenerationAndDistributionManagement
    {
        private RadzenDataGrid<DailyPowerGenerationAndDistribution> grid;
        private IEnumerable<DailyPowerGenerationAndDistribution> dailyPowerGenerationAndDistributions;
        private DailyPowerGenerationAndDistribution dailyPowerGenerationAndDistributionToInsert;
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
            var dailyPowerGenerationAndDistributionService = this.GetGenericService(serviceScope);
            var dailyPowerGenerationAndDistributionsResponse = await dailyPowerGenerationAndDistributionService.Get()
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DailyPowerGenerationAndDistribution>();

            count = (int)dailyPowerGenerationAndDistributionsResponse.Count;
            dailyPowerGenerationAndDistributions = dailyPowerGenerationAndDistributionsResponse.ToList();

            isLoading = false;
        }

        private async Task EditRow(DailyPowerGenerationAndDistribution dailyPowerGenerationAndDistribution)
        {
            await grid.EditRow(dailyPowerGenerationAndDistribution);
        }

        private async Task SaveRow(DailyPowerGenerationAndDistribution dailyPowerGenerationAndDistribution)
        {
            try
            {
                if (dailyPowerGenerationAndDistribution == dailyPowerGenerationAndDistributionToInsert)
                {
                    dailyPowerGenerationAndDistributionToInsert = null;
                }
                using var serviceScope = ServiceProvider.CreateScope();
                var dailyPowerGenerationAndDistributionService = this.GetGenericService(serviceScope);
                await dailyPowerGenerationAndDistributionService.UpdateAsync(dailyPowerGenerationAndDistribution, dailyPowerGenerationAndDistribution.Id);
                await grid.UpdateRow(dailyPowerGenerationAndDistribution);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        private void CancelEdit(DailyPowerGenerationAndDistribution dailyPowerGenerationAndDistribution)
        {
            if (dailyPowerGenerationAndDistribution == dailyPowerGenerationAndDistributionToInsert)
            {
                dailyPowerGenerationAndDistributionToInsert = null;
            }

            grid.CancelEditRow(dailyPowerGenerationAndDistribution);

        }

        private async Task DeleteRow(DailyPowerGenerationAndDistribution dailyPowerGenerationAndDistribution)
        {
            try
            {
                if (dailyPowerGenerationAndDistribution == dailyPowerGenerationAndDistributionToInsert)
                {
                    dailyPowerGenerationAndDistributionToInsert = null;
                }

                if (dailyPowerGenerationAndDistributions.Contains(dailyPowerGenerationAndDistribution))
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var dailyPowerGenerationAndDistributionService = this.GetGenericService(serviceScope);
                    await dailyPowerGenerationAndDistributionService.DeleteAsync(dailyPowerGenerationAndDistribution);
                    await grid.Reload();
                }
                else
                {
                    grid.CancelEditRow(dailyPowerGenerationAndDistribution);
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

        private IGenericService<DailyPowerGenerationAndDistribution> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyPowerGenerationAndDistribution, ICentralizedDatabaseSystemUnitOfWork>>();
        }
    }
}
