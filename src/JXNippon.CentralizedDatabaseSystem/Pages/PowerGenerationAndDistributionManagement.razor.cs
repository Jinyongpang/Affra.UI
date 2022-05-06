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
        private bool isLoading = false;
        private IEnumerable<PowerGenerator> powerGenerators;
        private string search = null;
        private DateTime? date = null;
        private string status = null;
        private IEnumerable<string> statuses = new string[] { "Online", "Offline", "Standby" };

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            powerGenerators = (await serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<PowerGenerator, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<PowerGenerator>()).ToList();
        }

        private async Task ReloadAsync()
        {
            grid.Reset(true);
            await grid.Reload();
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            using var serviceScope = ServiceProvider.CreateScope();
            var dailyPowerGenerationAndDistributionService = this.GetGenericService(serviceScope);
            var query = dailyPowerGenerationAndDistributionService.Get();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.PowerGeneratorName.ToUpper().Contains(search.ToUpper()));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status.ToUpper() == status.ToUpper());
            }
            if (date != null)
            {
                var start = TimeZoneInfo.ConvertTimeToUtc(date.Value);
                var end = TimeZoneInfo.ConvertTimeToUtc(date.Value.AddDays(1));
                query = query
                    .Where(x => x.Date >= start)
                    .Where(x => x.Date < end);
            }
            var dailyPowerGenerationAndDistributionsResponse = await query
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DailyPowerGenerationAndDistribution>();

            count = (int)dailyPowerGenerationAndDistributionsResponse.Count;
            dailyPowerGenerationAndDistributions = dailyPowerGenerationAndDistributionsResponse.ToList();

            isLoading = false;
        }

        private async Task EditRowAsync(DailyPowerGenerationAndDistribution dailyPowerGenerationAndDistribution)
        {
            await grid.EditRow(dailyPowerGenerationAndDistribution);
        }

        private async Task SaveRowAsync(DailyPowerGenerationAndDistribution dailyPowerGenerationAndDistribution)
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

        private async Task OnChangeAsync(object value, string name)
        {
            search = value.ToString();
            await this.ReloadAsync();
        }
        private async Task OnChangeStatusFilterAsync(object value, string name)
        {
            status = value?.ToString();
            await this.ReloadAsync();
        }

        private void OnTodayClick()
        {
            date = DateTime.Now;
        }

        private async Task OnChangeAsync(DateTime? value, string name, string format)
        {
            date = value;
            await this.ReloadAsync();
        }
    }
}
