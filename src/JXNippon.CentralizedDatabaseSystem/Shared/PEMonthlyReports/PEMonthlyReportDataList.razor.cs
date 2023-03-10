using System.Collections.Concurrent;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.AuditTrails;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.Loadings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Microsoft.OData.Client;
using NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages;
using Radzen;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.PEMonthlyReports
{
    public partial class PEMonthlyReportDataList
    {
        private int count;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
        private Virtualize<PEReport> virtualize;
        private IDictionary<string, User> users = new ConcurrentDictionary<string, User>(StringComparer.OrdinalIgnoreCase);

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IReportService ReportService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        public CommonFilter Filter { get; set; }

        protected async override Task OnInitializedAsync()
        {
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.WellAllocation), HasReadPermissoin = true, HasWritePermission = true });
            Filter = new CommonFilter(navigationManager);
        }
        public async Task ReloadAsync()
        {
            await virtualize.RefreshDataAsync();
            StateHasChanged();
        }

        private async ValueTask<ItemsProviderResult<PEReport>> LoadDataAsync(ItemsProviderRequest request)
        {
            isLoading = true;
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
                var peMonthlyReportsList = peMonthlyReportsResponse.ToList();
                foreach (var email in peMonthlyReportsList
                    .Where(x => !string.IsNullOrEmpty(x.User))
                    .Select(x => x.User))
                {
                    if (!this.users.TryGetValue(email, out var user))
                    {
                        using var serviceScopeUser = ServiceProvider.CreateScope();
                        var userService = this.GetUserGenericService(serviceScopeUser);
                        user = (await userService.Get()
                            .Where(x => x.Email.ToUpper() == email.ToUpper())
                            .ToQueryOperationResponseAsync<User>())
                            .FirstOrDefault();
                        this.users[email] = user;
                    }
                }
                return new ItemsProviderResult<PEReport>(peMonthlyReportsList, count);
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
            finally
            {
                initLoading = false;
                isLoading = false;
                StateHasChanged();
            }
            return new ItemsProviderResult<PEReport>(Array.Empty<PEReport>(), count);
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<PEReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<PEReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task ShowDialogAsync(PEReport data)
        {
            var peReportTask = GetFullPEReportAsync(data.Date);
            await DialogService.OpenAsync<LoadingMessage>("", new() { ["Message"] = "Retrieving report. Please wait...", ["Task"] = peReportTask }, Constant.LoadingDialogOptions);
            var peReport = await peReportTask;

            dynamic? dialogResponse;
            dialogResponse = await DialogService.OpenAsync<PEMonthlyReportView>(data.Date.ToLocalTime().ToString("yyyy MMMM"),
                       new Dictionary<string, object>() { { "Data", peReport } },
                       Constant.FullScreenDialogOptions);

            if (dialogResponse == true)
            {

            }

            await ReloadAsync();
        }

        private async Task DownloadAsync(PEReport input)
        {
            try
            {
                if (input.LastApproval is null)
                {
                    throw new InvalidOperationException("Report never approved before.");
                }
                var streamResult = await ReportService.DownloadReportAsync(input.LastApproval.ReportReferenceId);
                if (streamResult != null)
                {
                    using var streamRef = new DotNetStreamReference(streamResult);
                    await JSRuntime.InvokeVoidAsync("downloadFileFromStream", input.LastApproval.FileName, streamRef);
                }
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }

        private Task DownloadWithLoadingAsync(PEReport peReport)
        {
            var task = DownloadAsync(peReport);
            return DialogService.OpenAsync<LoadingMessage>("", new() { ["Message"] = "Generating report. Please wait...", ["Task"] = task }, Constant.LoadingDialogOptions);
        }

        private async Task<PEReport> GetFullPEReportAsync(DateTimeOffset date)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var service = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<PEReport, ICentralizedDatabaseSystemUnitOfWork>>();
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
        private IGenericService<User> GetUserGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<User, IUserUnitOfWork>>();
        }

        private string GetAvatarName(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return "NA";
            }
            if (this.users.TryGetValue(email, out var user)
                && user is not null)
            {
                return this.UserService.GetAvatarName(user.Name);
            }

            return $"{email[0]}";
        }

        private string GetAvatarColor(string email)
        {
            if (!string.IsNullOrEmpty(email) && this.users.TryGetValue(email, out var user))
            {
                return user?.UserPersonalization?.AvatarColor ?? string.Empty;
            }
            return string.Empty;
        }

        private string GetAvatarIcon(string email)
        {
            if (!string.IsNullOrEmpty(email) && this.users.TryGetValue(email, out var user))
            {
                return user?.UserPersonalization?.AvatarId > 0
                     ? $"avatar\\{user?.UserPersonalization?.AvatarId}.png"
                     : string.Empty;
            }
            return string.Empty;
        }

        private async Task ShowAuditTrailAsync(PEReport pEReport)
        {
            _ = await DialogService.OpenAsync<AuditTrailTable>(string.Format(System.Globalization.CultureInfo.CurrentCulture, "PE Report : {0:yyyy MMMM}", pEReport.DateUI),
                              new Dictionary<string, object>() { ["Id"] = pEReport.Id, ["TableName"] = typeof(PEReport).Name },
                              new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });
        }
    }
}
