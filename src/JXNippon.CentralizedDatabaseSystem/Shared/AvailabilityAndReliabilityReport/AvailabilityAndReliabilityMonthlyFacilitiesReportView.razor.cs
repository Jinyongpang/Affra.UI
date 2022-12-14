using System.Globalization;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.AvailabilityAndReliabilityReport;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.AvailabilityAndReliabilityReport
{
    public partial class AvailabilityAndReliabilityMonthlyFacilitiesReportView
    {
        private bool isCollapsed = false;
        private Menu menu;
        private MonthlyHIPAvailabilityAndReliabilityCalculation HIPItems { get; set; }
        private MonthlyLayangAvailabilityAndReliabilityCalculation LayangItems { get; set; }
        private MonthlyFPSOAvailabilityAndReliabilityCalculation FPSOItems { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        private List<int> YearList { get; set; }

        private bool monthlyUptimeIsEditing;
        private bool monthlyTargetIsEditing;
        protected override async Task OnInitializedAsync()
        {
            await this.LoadYearAsync();
            await this.LoadDataAsync();
        }
        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            return this.LoadDataAsync(Convert.ToInt16(menuItem.Key.Substring(0, 4)), Convert.ToInt16(menuItem.Key.Substring(4)));
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
        public async Task LoadDataAsync(int? year = null, int? month = null)
        {
            try
            {
                using var serviceScopeUser = ServiceProvider.CreateScope();
                var hipService = this.GetHIPGenericService(serviceScopeUser);
                var layangService = this.GetLayangGenericService(serviceScopeUser);
                var fpsoService = this.GetFPSOGenericService(serviceScopeUser);

                if (year is null && month is null)
                {
                    HIPItems = (await hipService.Get()
                        .Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month)
                        .ToQueryOperationResponseAsync<MonthlyHIPAvailabilityAndReliabilityCalculation>())
                        .FirstOrDefault();

                    LayangItems = (await layangService.Get()
                        .Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month)
                        .ToQueryOperationResponseAsync<MonthlyLayangAvailabilityAndReliabilityCalculation>())
                        .FirstOrDefault();

                    FPSOItems = (await fpsoService.Get()
                        .Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month)
                        .ToQueryOperationResponseAsync<MonthlyFPSOAvailabilityAndReliabilityCalculation>())
                        .FirstOrDefault();
                }
                else
                {
                    HIPItems = (await hipService.Get()
                        .Where(x => x.Date.Year == year && x.Date.Month == month)
                        .ToQueryOperationResponseAsync<MonthlyHIPAvailabilityAndReliabilityCalculation>())
                        .FirstOrDefault();

                    LayangItems = (await layangService.Get()
                        .Where(x => x.Date.Year == year && x.Date.Month == month)
                        .ToQueryOperationResponseAsync<MonthlyLayangAvailabilityAndReliabilityCalculation>())
                        .FirstOrDefault();

                    FPSOItems = (await fpsoService.Get()
                        .Where(x => x.Date.Year == year && x.Date.Month == month)
                        .ToQueryOperationResponseAsync<MonthlyFPSOAvailabilityAndReliabilityCalculation>())
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
        public async Task OnDataUpdateHandler(string year)
        {
            await this.LoadDataAsync(Convert.ToInt16(year.Substring(0, 4)), Convert.ToInt16(year.Substring(4, 1)));
        }
        public string[] GetMonthList()
        {
            return DateTimeFormatInfo.CurrentInfo.MonthNames;
        }
        public int GetMonthInteger(string month)
        {
            switch(month)
            {
                case "January":
                    return 1;
                case "February":
                    return 2;
                case "March":
                    return 3;
                case "April":
                    return 4;
                case "May":
                    return 5;
                case "June":
                    return 6;
                case "July":
                    return 7;
                case "August":
                    return 8;
                case "September":
                    return 9;
                case "October":
                    return 10;
                case "November":
                    return 11;
                case "December":
                    return 12;
                default: 
                    return 1;
            }
        }
        private async Task StartEdit(string valueName)
        {
            if (await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.AvailabilityAndReliability), HasReadPermissoin = true, HasWritePermission = true }))
            {
                if(valueName == "Uptime")
                {
                    monthlyUptimeIsEditing = true;
                }
                else if (valueName == "Target")
                {
                    monthlyTargetIsEditing = true;
                }
                this.StateHasChanged();
            }
        }
        private void MouseLeave()
        {
            monthlyUptimeIsEditing = false;
            monthlyTargetIsEditing = false;

            this.StateHasChanged();
        }
        private async Task StopEdit()
        {
            try
            {
                using var scope = ServiceProvider.CreateScope();
                var hipService = this.GetHIPGenericService(scope);
                var fpsoService = this.GetFPSOGenericService(scope);
                var layangService = this.GetLayangGenericService(scope);

                FPSOItems.MonthlyActualPlannedUptime = HIPItems.MonthlyActualPlannedUptime;
                FPSOItems.MonthlyTarget = HIPItems.MonthlyTarget;

                LayangItems.MonthlyActualPlannedUptime = HIPItems.MonthlyActualPlannedUptime;
                LayangItems.MonthlyTarget = HIPItems.MonthlyTarget;

                await hipService.UpdateAsync(this.HIPItems, this.HIPItems.Id);
                await fpsoService.UpdateAsync(this.FPSOItems, this.FPSOItems.Id);
                await layangService.UpdateAsync(this.LayangItems, this.LayangItems.Id);
                AffraNotificationService.NotifyItemUpdated();

                monthlyUptimeIsEditing = false;
                monthlyTargetIsEditing = false;
                this.StateHasChanged();
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }
        private decimal GetMonthlyAvailabilityAverage()
        {
            return Math.Round(((this.HIPItems.AvailabilityPercentage.Value + this.FPSOItems.AvailabilityPercentage.Value + this.LayangItems.AvailabilityPercentage.Value) / 3), 2);
        }
    }
}
