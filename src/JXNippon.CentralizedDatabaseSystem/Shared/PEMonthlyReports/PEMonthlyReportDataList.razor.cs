using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.PEMonthlyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Domain.Workspaces;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.Loadings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Microsoft.OData.Client;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.PEMonthlyReports
{
    public partial class PEMonthlyReportDataList
    {
        private int count;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
        private Virtualize<PEReport> virtualize;

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IReportService ReportService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        public CommonFilter Filter { get; set; }

        protected override Task OnInitializedAsync()
        {
            Filter = new CommonFilter(navigationManager);
            return Task.CompletedTask;
        }
        public async Task ReloadAsync()
        {
            await virtualize.RefreshDataAsync();
            this.StateHasChanged();
        }

        private async ValueTask<ItemsProviderResult<PEReport>> LoadDataAsync(ItemsProviderRequest request)
        {
            isLoading = true;
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.WellAllocation), HasReadPermissoin = true, HasWritePermission = true });
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<PEReport>? peMonthlyReportService = GetGenericService(serviceScope);
                var query = peMonthlyReportService.Get();
                if (Filter.Status != null)
                {
                    var status = (PEReportStatus)Enum.Parse(typeof(PEReportStatus), Filter.Status);
                    query = query.Where(PEMonthlyReport => PEMonthlyReport.Status == status);
                }
                if (Filter.DateRange?.Start != null)
                {
                    var start = Filter.DateRange.Start.ToUniversalTime();
                    var end = Filter.DateRange.End.ToUniversalTime();
                    query = query
                        .Where(PEMonthlyReport => PEMonthlyReport.Date >= start)
                        .Where(PEMonthlyReport => PEMonthlyReport.Date <= end);
                }

                Microsoft.OData.Client.QueryOperationResponse<PEReport>? peMonthlyReportsResponse = await query
                    .OrderByDescending(peMonthlyReport => peMonthlyReport.Date)
                    .Skip(request.StartIndex)
                    .Take(request.Count)
                    .ToQueryOperationResponseAsync<PEReport>();

                count = (int)peMonthlyReportsResponse.Count;
                var combinedDailyReportsList = peMonthlyReportsResponse.ToList();

                return new ItemsProviderResult<PEReport>(combinedDailyReportsList, count);
            }
            finally
            {
                initLoading = false;
                isLoading = false;
                StateHasChanged();
            }
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<PEReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<PEReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        private async Task DownloadAsync(PEReport input)
        {
            try
            {
                var peReport = await this.GetFullPEReportAsync(input.Date);
                var streamResult = await ReportService.GeneratePEReportAsync(peReport);
                if (streamResult != null)
                {
                    using var streamRef = new DotNetStreamReference(streamResult);
                    await JSRuntime.InvokeVoidAsync("downloadFileFromStream", $"PEReport{peReport.Date.ToLocalTime():yyyy MMMM}.xlsx", streamRef);
                }
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }
        private Task DownloadWithLoadingAsync(PEReport peReport)
        {
            var task = this.DownloadAsync(peReport);
            return this.DialogService.OpenAsync<LoadingMessage>("", new() { ["Message"] = "Generating report. Please wait...", ["Task"] = task }, Constant.LoadingDialogOptions);
        }

        private async Task<PEReport> GetFullPEReportAsync(DateTimeOffset date)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var service = serviceScope.ServiceProvider.GetRequiredService<IGenericService<PEReport>>();
            var query = (DataServiceQuery<PEReport>)service.Get();
            var response = await ((DataServiceQuery<PEReport>)query
                .Expand(x => x.DailyHIPSales)
                .Expand(x => x.DailyFPSOSales)
                .Expand(x => x.DailyGasMeterings)
                .Expand(x => x.DailyCondensateCalculateds)
                .Expand(x => x.MonthlyHIPSale)
                .Expand(x => x.MonthlyFPSOSale)
                .Expand(x => x.DailyHIPFieldDs)
                .Expand(x => x.DailyFPSOFieldDs)
                .Expand(x => x.MonthlyHIPFieldMY)
                .Expand(x => x.MonthlyFPSOFieldMY)
                .Expand(x => x.MonthlyReservoirs)
                .Expand(x => x.MonthlyReservoirProductions)
                .Expand(x => x.MonthlyWellProductions)
                .Expand(x => x.MonthlyWellTests)
                .Expand(x => x.DailyEstimatedWellGasProductions)
                .Expand(x => x.DailyAllocatedWellGasProductions)
                .Expand(x => x.DailyEstimatedWellCondensateProductions)
                .Expand(x => x.DailyAllocatedWellCondensateProductions)
                .Expand(x => x.DailyEstimatedWellWaterProductions)
                .Expand(x => x.DailyAllocatedWellWaterProductions)
                .Expand(x => x.DailyHL1WellProductionCalculations)
                .Expand(x => x.DailyHL2WellProductionCalculations)
                .Expand(x => x.DailyHL3WellProductionCalculations)
                .Expand(x => x.DailyHL4WellProductionCalculations)
                .Expand(x => x.DailyHL5WellProductionCalculations)
                .Expand(x => x.DailyHL6WellProductionCalculations)
                .Expand(x => x.DailyHL7WellProductionCalculations)
                .Expand(x => x.DailyHL8WellProductionCalculations)
                .Expand(x => x.DailyHL9WellProductionCalculations)
                .Expand(x => x.DailyHL10WellProductionCalculations)
                .Expand(x => x.DailyHL11WellProductionCalculations)
                .Expand(x => x.DailyHL12WellProductionCalculations)
                .Expand(x => x.DailyHM1WellProductionCalculations)
                .Expand(x => x.DailyHM2WellProductionCalculations)
                .Expand(x => x.DailyHM3WellProductionCalculations)
                .Expand(x => x.DailyHM4WellProductionCalculations)
                .Expand(x => x.DailyHM5WellProductionCalculations)
                .Expand(x => x.DailyLA1WellProductionCalculations)
                .Expand(x => x.DailyLA2WellProductionCalculations)
                .Expand(x => x.DailyLA3WellProductionCalculations)
                .Expand(x => x.DailyBU1ST1WellProductionCalculations)
                .Expand(x => x.DailyBU2WellProductionCalculations)
                .Expand(x => x.DailyBU3WellProductionCalculations)
                .Where(x => x.Date == date))
                .ExecuteAsync();

            var cdrItem = response.FirstOrDefault();

            return cdrItem;
        }

    }
}
