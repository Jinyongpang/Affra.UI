using System.Globalization;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.AvailabilityAndReliabilityReport;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class MonthlyAvailabilityAndReliabilityReportView
    {
        private bool isCollapsed = false;
        private Menu menu;
        private MonthlyHIPAvailabilityAndReliabilityCalculation HIPItems { get; set; }
        private MonthlyLayangAvailabilityAndReliabilityCalculation LayangItems { get; set; }
        private MonthlyFPSOAvailabilityAndReliabilityCalculation FPSOItems { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
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
        private IGenericService<MonthlyHIPAvailabilityAndReliabilityCalculation> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<MonthlyHIPAvailabilityAndReliabilityCalculation, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        public async Task LoadDataAsync(int? selectedYear = null)
        {
            using var serviceScopeUser = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScopeUser);

            if (selectedYear is null)
            {
                HIPItems = (await service.Get()
                    .Where(x => x.Date.Year == DateTime.Now.Year)
                    .ToQueryOperationResponseAsync<MonthlyHIPAvailabilityAndReliabilityCalculation>())
                    .FirstOrDefault();
                LayangItems = (await service.Get()
                    .Where(x => x.Date.Year == DateTime.Now.Year)
                    .ToQueryOperationResponseAsync<MonthlyLayangAvailabilityAndReliabilityCalculation>())
                    .FirstOrDefault();
                FPSOItems = (await service.Get()
                    .Where(x => x.Date.Year == DateTime.Now.Year)
                    .ToQueryOperationResponseAsync<MonthlyFPSOAvailabilityAndReliabilityCalculation>())
                    .FirstOrDefault();
            }
            else
            {
                HIPItems = (await service.Get()
                    .Where(x => x.Date.Year == selectedYear)
                    .ToQueryOperationResponseAsync<MonthlyHIPAvailabilityAndReliabilityCalculation>())
                    .FirstOrDefault();
                LayangItems = (await service.Get()
                    .Where(x => x.Date.Year == selectedYear)
                    .ToQueryOperationResponseAsync<MonthlyLayangAvailabilityAndReliabilityCalculation>())
                    .FirstOrDefault();
                FPSOItems = (await service.Get()
                    .Where(x => x.Date.Year == selectedYear)
                    .ToQueryOperationResponseAsync<MonthlyFPSOAvailabilityAndReliabilityCalculation>())
                    .FirstOrDefault();
            }

            StateHasChanged();
        }
        public async Task LoadYearAsync(int? selectedYear = null)
        {
            using var serviceScopeUser = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScopeUser);

            YearList = (await service.Get()
                .ToQueryOperationResponseAsync<MonthlyHIPAvailabilityAndReliabilityCalculation>())
                .Select(x => x.Date.Year)
                .OrderDescending()
                .ToList();

            StateHasChanged();
        }
        public async Task OnDataUpdateHandler(string year)
        {
            await this.LoadDataAsync(Convert.ToInt16(year));
        }
        public string[] GetMonthList()
        {
            return DateTimeFormatInfo.CurrentInfo.MonthNames;
        }
    }
}
