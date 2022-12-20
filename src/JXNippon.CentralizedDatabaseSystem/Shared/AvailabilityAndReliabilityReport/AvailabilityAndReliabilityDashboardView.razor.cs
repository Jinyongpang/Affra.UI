using System.Reflection;
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
        private YearlyHIPAvailabilityCalculation HIPAvailabilityItems { get; set; }
        private YearlyHIPReliabilityCalculation HIPReliabilityItems { get; set; }
        private YearlyFPSOAvailabilityCalculation FPSOAvailabilityItems { get; set; }
        private YearlyFPSOReliabilityCalculation FPSOReliabilityItems { get; set; }
        private YearlyLayangAvailabilityCalculation LayangAvailabilityItems { get; set; }
        private YearlyLayangReliabilityCalculation LayangReliabilityItems { get; set; }
        private YearlyAverageAvailabilityCalculation AverageAvailabilityItems { get; set; }
        private YearlyAverageReliabilityCalculation AverageReliabilityItems { get; set; }
        private YearlyTargetCalculation TargetItems { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        private List<int> YearList { get; set; }

        private string[] PropertyNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "YTD", "YEP", "Q1", "Q2", "Q3", "Q4" };
        protected override async Task OnInitializedAsync()
        {
            await this.LoadYearAsync();
            await this.LoadDataAsync();
        }
        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            return this.LoadDataAsync(Convert.ToInt16(menuItem.Key));
        }
        private IGenericService<YearlyHIPAvailabilityCalculation> GetHIPAvailabilityGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyHIPAvailabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<YearlyHIPReliabilityCalculation> GetHIPReliabilityGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyHIPReliabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<YearlyFPSOAvailabilityCalculation> GetFPSOAvailabilityGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyFPSOAvailabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<YearlyFPSOReliabilityCalculation> GetFPSOReliabilityGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyFPSOReliabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<YearlyLayangAvailabilityCalculation> GetLayangAvailabilityGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyLayangAvailabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<YearlyLayangReliabilityCalculation> GetLayangReliabilityGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyLayangReliabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<YearlyAverageAvailabilityCalculation> GetAverageAvailabilityGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyAverageAvailabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<YearlyAverageReliabilityCalculation> GetAverageReliabilityGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyAverageReliabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private IGenericService<YearlyTargetCalculation> GetTargetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<YearlyTargetCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        public async Task LoadDataAsync(int? year = null)
        {
            try
            {
                using var serviceScopeUser = ServiceProvider.CreateScope();
                var hipAvailabilityService = this.GetHIPAvailabilityGenericService(serviceScopeUser);
                var hipReliabilityService = this.GetHIPReliabilityGenericService(serviceScopeUser);
                var fpsoAvailabilityService = this.GetFPSOAvailabilityGenericService(serviceScopeUser);
                var fpsoReliabilityService = this.GetFPSOReliabilityGenericService(serviceScopeUser);
                var layangAvailabilityService = this.GetLayangAvailabilityGenericService(serviceScopeUser);
                var layangReliabilityService = this.GetLayangReliabilityGenericService(serviceScopeUser);
                var averageAvailabilityService = this.GetAverageAvailabilityGenericService(serviceScopeUser);
                var averageReliabilityService = this.GetAverageReliabilityGenericService(serviceScopeUser);
                var targetService = this.GetTargetGenericService(serviceScopeUser);

                if (year is null)
                {
                    HIPAvailabilityItems = (await hipAvailabilityService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyHIPAvailabilityCalculation>())
                        .FirstOrDefault();

                    HIPReliabilityItems = (await hipReliabilityService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyHIPReliabilityCalculation>())
                        .FirstOrDefault();

                    FPSOAvailabilityItems = (await fpsoAvailabilityService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyFPSOAvailabilityCalculation>())
                        .FirstOrDefault();

                    FPSOReliabilityItems = (await fpsoReliabilityService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyFPSOReliabilityCalculation>())
                        .FirstOrDefault();

                    LayangAvailabilityItems = (await layangAvailabilityService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyLayangAvailabilityCalculation>())
                        .FirstOrDefault();

                    LayangReliabilityItems = (await layangReliabilityService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyLayangReliabilityCalculation>())
                        .FirstOrDefault();

                    AverageAvailabilityItems = (await averageAvailabilityService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyAverageAvailabilityCalculation>())
                        .FirstOrDefault();

                    AverageReliabilityItems = (await averageReliabilityService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyAverageReliabilityCalculation>())
                        .FirstOrDefault();

                    TargetItems = (await targetService.Get()
                        .Where(x => x.Year == DateTime.Now.Year)
                        .ToQueryOperationResponseAsync<YearlyTargetCalculation>())
                        .FirstOrDefault();
                }
                else
                {
                    HIPAvailabilityItems = (await hipAvailabilityService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyHIPAvailabilityCalculation>())
                        .FirstOrDefault();

                    HIPReliabilityItems = (await hipReliabilityService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyHIPReliabilityCalculation>())
                        .FirstOrDefault();

                    FPSOAvailabilityItems = (await fpsoAvailabilityService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyFPSOAvailabilityCalculation>())
                        .FirstOrDefault();

                    FPSOReliabilityItems = (await fpsoReliabilityService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyFPSOReliabilityCalculation>())
                        .FirstOrDefault();

                    LayangAvailabilityItems = (await layangAvailabilityService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyLayangAvailabilityCalculation>())
                        .FirstOrDefault();

                    LayangReliabilityItems = (await layangReliabilityService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyLayangReliabilityCalculation>())
                        .FirstOrDefault();

                    AverageAvailabilityItems = (await averageAvailabilityService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyAverageAvailabilityCalculation>())
                        .FirstOrDefault();

                    AverageReliabilityItems = (await averageReliabilityService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyAverageReliabilityCalculation>())
                        .FirstOrDefault();

                    TargetItems = (await targetService.Get()
                        .Where(x => x.Year == year)
                        .ToQueryOperationResponseAsync<YearlyTargetCalculation>())
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
                var service = this.GetHIPAvailabilityGenericService(serviceScopeUser);

                YearList = (await service.Get()
                    .ToQueryOperationResponseAsync<YearlyHIPAvailabilityCalculation>())
                    .Select(x => x.Year)
                    .OrderDescending()
                    .ToList();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }

            StateHasChanged();
        }
        public double GetYearlyPropertyValue(object yearlyItem, string month)
        {
            Console.WriteLine($"Month : {month}");
            if (yearlyItem is null)
            {
                return 100.00;
            }
            Type t = yearlyItem.GetType();
            PropertyInfo info = t.GetProperty(month);

            if (info is null)
            {
                return 100.00;
            }

            if (!info.CanRead)
            {
                return 100.00;
            }
            Console.WriteLine($"{month} : {Convert.ToDouble(info.GetValue(yearlyItem, null))}");
            return Convert.ToDouble(info.GetValue(yearlyItem, null));
        }
    }
}
