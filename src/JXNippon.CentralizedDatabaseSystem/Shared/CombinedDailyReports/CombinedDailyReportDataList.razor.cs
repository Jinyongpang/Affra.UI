using System.Collections.Concurrent;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
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
using Radzen;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportDataList
    {
        private int count;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
        private Virtualize<CombinedDailyReport> virtualize;
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
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.CombinedDailyReport), HasReadPermissoin = true, HasWritePermission = true });
            Filter = new CommonFilter(navigationManager);
        }
        public async Task ReloadAsync()
        {
            await virtualize.RefreshDataAsync();
            StateHasChanged();
        }

        private async ValueTask<ItemsProviderResult<CombinedDailyReport>> LoadDataAsync(ItemsProviderRequest request)
        {
            isLoading = true;
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
                foreach (var email in combinedDailyReportsList
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
                return new ItemsProviderResult<CombinedDailyReport>(combinedDailyReportsList, count);
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
            return new ItemsProviderResult<CombinedDailyReport>(Array.Empty<CombinedDailyReport>(), count);
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
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var cdrService = serviceScope.ServiceProvider.GetRequiredService<ICombinedDailyReportService>();
                var cdrItemTask = cdrService.GetCombinedDailyReportAsync(data.Date);
                await DialogService.OpenAsync<LoadingMessage>("", new() { ["Message"] = "Retrieving report. Please wait...", ["Task"] = cdrItemTask }, Constant.LoadingDialogOptions);
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
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private Task DownloadWithLoadingAsync(CombinedDailyReport combinedDailyReport)
        {
            var task = DownloadAsync(combinedDailyReport);
            return DialogService.OpenAsync<LoadingMessage>("", new() { ["Message"] = "Generating report. Please wait...", ["Task"] = task }, Constant.LoadingDialogOptions);
        }

        private async Task DownloadAsync(CombinedDailyReport combinedDailyReport)
        {
            try
            {
                if (combinedDailyReport.LastApproval is null)
                {
                    throw new InvalidOperationException("Report never approved before.");
                }
                var streamResult = await ReportService.DownloadReportAsync(combinedDailyReport.LastApproval.ReportReferenceId);
                if (streamResult != null)
                {
                    using var streamRef = new DotNetStreamReference(streamResult);
                    await JSRuntime.InvokeVoidAsync("downloadFileFromStream", combinedDailyReport.LastApproval.FileName, streamRef);
                }
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
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

        private async Task ShowAuditTrailAsync(CombinedDailyReport combinedDailyReport)
        {
            _ = await DialogService.OpenAsync<AuditTrailTable>($"Combined Daily Report : {combinedDailyReport.DateUI.ToDateString()}",
                              new Dictionary<string, object>() { ["Id"] = combinedDailyReport.Id, ["TableName"] = typeof(CombinedDailyReport).Name },
                              new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });
        }
    }
}
