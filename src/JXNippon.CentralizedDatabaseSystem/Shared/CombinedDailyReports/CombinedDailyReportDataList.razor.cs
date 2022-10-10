using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportDataList
    {
        private int count;
        private bool isLoading = false;
        private bool initLoading = true;
        private Virtualize<CombinedDailyReport> virtualize;

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        public CommonFilter Filter { get; set; }

        protected override Task OnInitializedAsync()
        {
            Filter = new CommonFilter(navigationManager);
            initLoading = false;
            return Task.CompletedTask;
        }
        public async Task ReloadAsync()
        {
            await virtualize.RefreshDataAsync();
            this.StateHasChanged();
        }

        private async ValueTask<ItemsProviderResult<CombinedDailyReport>> LoadDataAsync(ItemsProviderRequest request)
        {
            isLoading = true;

            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<CombinedDailyReport>? combinedDailyReportService = GetGenericFileService(serviceScope);
                var query = combinedDailyReportService.Get();
                if (Filter.Status != null)
                {
                    var status = (CombinedDailyReportStatus)Enum.Parse(typeof(CombinedDailyReportStatus), Filter.Status);
                    query = query.Where(CombinedDailyReport => CombinedDailyReport.Status == status);
                }
                if (Filter.Date != null)
                {
                    var start = TimeZoneInfo.ConvertTimeToUtc(Filter.Date.Value);
                    var end = TimeZoneInfo.ConvertTimeToUtc(Filter.Date.Value.AddDays(1));
                    query = query
                        .Where(CombinedDailyReport => CombinedDailyReport.Date >= start)
                        .Where(CombinedDailyReport => CombinedDailyReport.Date < end);
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
                isLoading = false;
                StateHasChanged();
            }
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<CombinedDailyReport> GetGenericFileService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<CombinedDailyReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }
    }
}
