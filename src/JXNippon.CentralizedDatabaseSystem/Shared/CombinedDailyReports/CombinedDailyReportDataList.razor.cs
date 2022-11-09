using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.Loadings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportDataList
    {
        private int count;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
        private Virtualize<CombinedDailyReport> virtualize;

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
            StateHasChanged();
        }

        private async ValueTask<ItemsProviderResult<CombinedDailyReport>> LoadDataAsync(ItemsProviderRequest request)
        {
            isLoading = true;
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.CombinedDailyReport), HasReadPermissoin = true, HasWritePermission = true });
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<CombinedDailyReport>? combinedDailyReportService = GetGenericService(serviceScope);
                var query = combinedDailyReportService.Get();
                if (Filter.Status != null)
                {
                    var status = (CombinedDailyReportStatus)Enum.Parse(typeof(CombinedDailyReportStatus), Filter.Status);
                    query = query.Where(CombinedDailyReport => CombinedDailyReport.Status == status);
                }
                if (Filter.DateRange?.Start != null)
                {
                    var start = Filter.DateRange.Start.ToUniversalTime();
                    var end = Filter.DateRange.End.ToUniversalTime();
                    query = query
                        .Where(CombinedDailyReport => CombinedDailyReport.Date >= start)
                        .Where(CombinedDailyReport => CombinedDailyReport.Date <= end);
                }

                Microsoft.OData.Client.QueryOperationResponse<CombinedDailyReport>? combinedDailyReportsResponse = await query
                    .OrderByDescending(combinedDailyReport => combinedDailyReport.Date)
                    .Skip(request.StartIndex)
                    .Take(request.Count)
                    .ToQueryOperationResponseAsync<CombinedDailyReport>();

                count = (int)combinedDailyReportsResponse.Count;
                var combinedDailyReportsList = combinedDailyReportsResponse.ToList();

                return new ItemsProviderResult<CombinedDailyReport>(combinedDailyReportsList, count);
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

        private IGenericService<CombinedDailyReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<CombinedDailyReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task ShowDialogAsync(CombinedDailyReport data)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var cdrService = serviceScope.ServiceProvider.GetRequiredService<ICombinedDailyReportService>();
            var cdrItemTask = cdrService.GetCombinedDailyReportAsync(data.Date);
            await this.DialogService.OpenAsync<LoadingMessage>("", new() { ["Message"] = "Retrieving report. Please wait...", ["Task"] = cdrItemTask }, Constant.LoadingDialogOptions);
            var cdrItem = await cdrItemTask;
            if (cdrItem is not null)
            {
                cdrItem.DailyHIPAndLWPSummarys = cdrService.AppendSummary(cdrItem.DailyHIPAndLWPSummarys, cdrItem);
                cdrItem.DailyFPSOHelangSummarys = cdrService.AppendSummary(cdrItem.DailyFPSOHelangSummarys, cdrItem);
            }

            dynamic? dialogResponse;
            dialogResponse = await DialogService.OpenAsync<CombinedDailyReportView>(data.Date.ToLocalTime().ToString("d"),
                       new Dictionary<string, object>() { { "Data", cdrItem } },
                       Constant.FullScreenDialogOptions);

            if (dialogResponse == true)
            {

            }

            await ReloadAsync();
        }

        private Task DownloadWithLoadingAsync(CombinedDailyReport combinedDailyReport)
        {
            var task = this.DownloadAsync(combinedDailyReport);
            return this.DialogService.OpenAsync<LoadingMessage>("", new() { ["Message"] = "Generating report. Please wait...", ["Task"] = task }, Constant.LoadingDialogOptions);
        }

        private async Task DownloadAsync(CombinedDailyReport combinedDailyReport)
        {
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var cdrService = serviceScope.ServiceProvider.GetRequiredService<ICombinedDailyReportService>();
                var cdrItem = await cdrService.GetFullCombinedDailyReportAsync(combinedDailyReport.Date);
                if (cdrItem is not null)
                {
                    cdrItem.DailyHIPAndLWPSummarys = cdrService.AppendSummary(cdrItem.DailyHIPAndLWPSummarys, cdrItem);
                    cdrItem.DailyFPSOHelangSummarys = cdrService.AppendSummary(cdrItem.DailyFPSOHelangSummarys, cdrItem);
                }
                var streamResult = await ReportService.GenerateCombinedDailyReportReportAsync(cdrItem);
                if (streamResult != null)
                {
                    using var streamRef = new DotNetStreamReference(streamResult);
                    await JSRuntime.InvokeVoidAsync("downloadFileFromStream", $"CombinedDailyReport_{combinedDailyReport.Date.ToLocalTime():d}.xlsx", streamRef);
                }
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }
    }
}
