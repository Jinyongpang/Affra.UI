using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.PEMonthlyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
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
    }
}
