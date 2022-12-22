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
        public double Percentage { get; set; }
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

        private List<DataItem> HIPAvailabilityChartList { get; set; }
        private List<DataItem> HIPReliabilityChartList { get; set; }
        private List<DataItem> FPSOAvailabilityChartList { get; set; }
        private List<DataItem> FPSOReliabilityChartList { get; set; }
        private List<DataItem> LayangAvailabilityChartList { get; set; }
        private List<DataItem> LayangReliabilityChartList { get; set; }
        private List<DataItem> AverageAvailabilityChartList { get; set; }
        private List<DataItem> AverageReliabilityChartList { get; set; }
        private List<DataItem> TargetChartList { get; set; }
        private List<double> HIPAvailabilityPercentageList { get; set; }
        private List<double> HIPReliabilityPercentageList { get; set; }
        private List<double> FPSOAvailabilityPercentageList { get; set; }
        private List<double> FPSOReliabilityPercentageList { get; set; }
        private List<double> LayangAvailabilityPercentageList { get; set; }
        private List<double> LayangReliabilityPercentageList { get; set; }
        private List<double> AverageAvailabilityPercentageList { get; set; }
        private List<double> AverageReliabilityPercentageList { get; set; }
        private List<double> TargetPercentageList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.LoadYearAsync();
            await this.LoadDataAsync();
            await this.LoadListAsync();
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
        public async Task LoadListAsync()
        {
            if (HIPAvailabilityItems is not null)
            {
                HIPAvailabilityChartList = new List<DataItem>();
                HIPAvailabilityPercentageList = new List<double>();

                CreateListData(HIPAvailabilityItems, HIPAvailabilityChartList, HIPAvailabilityPercentageList);
            }

            if (HIPReliabilityItems is not null)
            {
                HIPReliabilityChartList = new List<DataItem>();
                HIPReliabilityPercentageList = new List<double>();

                CreateListData(HIPReliabilityItems, HIPReliabilityChartList, HIPReliabilityPercentageList);
            }

            if (FPSOAvailabilityItems is not null)
            {
                FPSOAvailabilityChartList = new List<DataItem>();
                FPSOAvailabilityPercentageList = new List<double>();

                CreateListData(FPSOAvailabilityItems, FPSOAvailabilityChartList, FPSOAvailabilityPercentageList);
            }

            if (FPSOReliabilityItems is not null)
            {
                FPSOReliabilityChartList = new List<DataItem>();
                FPSOReliabilityPercentageList = new List<double>();

                CreateListData(FPSOReliabilityItems, FPSOReliabilityChartList, FPSOReliabilityPercentageList);
            }


            if (LayangAvailabilityItems is not null)
            {
                LayangAvailabilityChartList = new List<DataItem>();
                LayangAvailabilityPercentageList = new List<double>();

                CreateListData(LayangAvailabilityItems, LayangAvailabilityChartList, LayangAvailabilityPercentageList);
            }

            if (LayangReliabilityItems is not null)
            {
                LayangReliabilityChartList = new List<DataItem>();
                LayangReliabilityPercentageList = new List<double>();

                CreateListData(LayangReliabilityItems, LayangReliabilityChartList, LayangReliabilityPercentageList);
            }

            if (AverageAvailabilityItems is not null)
            {
                AverageAvailabilityChartList = new List<DataItem>();
                AverageAvailabilityPercentageList = new List<double>();

                CreateListData(AverageAvailabilityItems, AverageAvailabilityChartList, AverageAvailabilityPercentageList);
            }

            if (AverageReliabilityItems is not null)
            {
                AverageReliabilityChartList = new List<DataItem>();
                AverageReliabilityPercentageList = new List<double>();

                CreateListData(AverageReliabilityItems, AverageReliabilityChartList, AverageReliabilityPercentageList);
            }

            if (TargetItems is not null)
            {
                TargetChartList = new List<DataItem>();
                TargetPercentageList = new List<double>();

                CreateListData(TargetItems, TargetChartList, TargetPercentageList);
            }
        }
        private void CreateListData(object yearlyItem, List<DataItem> chartList, List<double> percentageList)
        {
            double percentage = 0.00;

            if (yearlyItem is not null)
            {
                percentage = 0.00;

                for (int i = 0; i < PropertyNames.Length; i++)
                {
                    percentage = GetYearlyPropertyValue(yearlyItem, PropertyNames[i].ToString());

                    percentageList.Add(percentage);

                    if (i < 12)
                    {
                        chartList.Add(new DataItem { Month = PropertyNames[i], Percentage = percentage });
                    }
                }
            }
        }
        public double GetYearlyPropertyValue(object yearlyItem, string month)
        {
            try
            {
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

                return Convert.ToDouble(info.GetValue(yearlyItem, null));
            }
            catch (Exception ex)
            {
                return 100.00;
                this.AffraNotificationService.NotifyException(ex);
            }

        }
    }
}
