using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.AvailabilityAndReliabilityReport;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    class DataItem
    {
        public string Month { get; set; }
        public decimal Percentage { get; set; }
    }
    public partial class AvailabilityAndReliabilityDashboardView
    {
        private bool isCollapsed = false;
        private Menu menu;
        private MonthlyHIPAvailabilityAndReliabilityCalculation HIPItems { get; set; }
        private MonthlyLayangAvailabilityAndReliabilityCalculation LayangItems { get; set; }
        private MonthlyFPSOAvailabilityAndReliabilityCalculation FPSOItems { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        private List<int> YearList { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await this.LoadYearAsync();
            await this.LoadDataAsync();
        }
        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            return this.LoadDataAsync(Convert.ToInt16(menuItem.Key));
        }
        private IGenericService<MonthlyHIPAvailabilityAndReliabilityCalculation> GetHIPGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<MonthlyHIPAvailabilityAndReliabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<MonthlyLayangAvailabilityAndReliabilityCalculation> GetLayangGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<MonthlyLayangAvailabilityAndReliabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<MonthlyFPSOAvailabilityAndReliabilityCalculation> GetFPSOGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<MonthlyFPSOAvailabilityAndReliabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        public async Task LoadDataAsync(int? year = null)
        {
            try
            {
                using var serviceScopeUser = ServiceProvider.CreateScope();
                var hipService = this.GetHIPGenericService(serviceScopeUser);
                var layangService = this.GetLayangGenericService(serviceScopeUser);
                var fpsoService = this.GetFPSOGenericService(serviceScopeUser);

                if (year is null)
                {
                    HIPItems = (await hipService.Get()
                        .Where(x => x.Date.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<MonthlyHIPAvailabilityAndReliabilityCalculation>())
                        .FirstOrDefault();
                }
                else
                {
                    HIPItems = (await hipService.Get()
                        .Where(x => x.Date.Year == year)
                        .ToQueryOperationResponseAsync<MonthlyHIPAvailabilityAndReliabilityCalculation>())
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }

            StateHasChanged();
        }
        public async Task LoadYearAsync(int? selectedYear = null)
        {
            try
            {
                using var serviceScopeUser = ServiceProvider.CreateScope();
                var service = this.GetHIPGenericService(serviceScopeUser);

                YearList = (await service.Get()
                    .ToQueryOperationResponseAsync<MonthlyHIPAvailabilityAndReliabilityCalculation>())
                    .Select(x => x.Date.Year)
                    .OrderDescending()
                    .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }

            StateHasChanged();
        }
        private decimal GetMonthlyAvailabilityAverage()
        {
            return Math.Round(((this.HIPItems.AvailabilityPercentage.Value + this.FPSOItems.AvailabilityPercentage.Value + this.LayangItems.AvailabilityPercentage.Value) / 3), 2);
        }
        private void ExtractMonthlyHIPItem(List<MonthlyHIPAvailabilityAndReliabilityCalculation> hipList)
        {

        }
    }
}
