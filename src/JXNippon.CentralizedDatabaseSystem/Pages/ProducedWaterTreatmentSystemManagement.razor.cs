using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class ProducedWaterTreatmentSystemManagement
    {
        private RadzenDataGrid<DailyProducedWaterTreatmentSystem> grid;
        private IEnumerable<DailyProducedWaterTreatmentSystem> dailyProducedWaterTreatmentSystems;
        private DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystemToInsert;
        private int count;
        private bool isLoading;
        private IEnumerable<ProducedWaterTreatmentSystem> producedWaterTreatmentSystems;
        private string search = null;
        private DateTime? date = null;
        private string status = null;
        private IEnumerable<string> statuses = new string[] { "Online", "Offline", "Standby" };

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            search = NavManager.GetQueryString<string>(nameof(search));
            date = NavManager.GetQueryString<DateTime?>(nameof(date));
            status = NavManager.GetQueryString<string>(nameof(status));

            using var serviceScope = ServiceProvider.CreateScope();
            producedWaterTreatmentSystems = (await serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<ProducedWaterTreatmentSystem, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<ProducedWaterTreatmentSystem>()).ToList();
        }

        private async Task ReloadAsync()
        {
            grid.Reset(true);
            await grid.Reload();
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            using var serviceScope = ServiceProvider.CreateScope();
            var dailyProducedWaterTreatmentSystemService = this.GetGenericService(serviceScope);
            var query = dailyProducedWaterTreatmentSystemService.Get();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.ProducedWaterTreatmentSystemName.ToUpper().Contains(search.ToUpper()));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status.ToUpper() == status.ToUpper());
            }
            if (date != null)
            {
                var start = TimeZoneInfo.ConvertTimeToUtc(date.Value);
                var end = TimeZoneInfo.ConvertTimeToUtc(date.Value.AddDays(1));
                query = query
                    .Where(x => x.Date >= start)
                    .Where(x => x.Date < end);
            }

            var dailyProducedWaterTreatmentSystemsResponse = await query
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DailyProducedWaterTreatmentSystem>();

            count = (int)dailyProducedWaterTreatmentSystemsResponse.Count;
            dailyProducedWaterTreatmentSystems = dailyProducedWaterTreatmentSystemsResponse.ToList();

            isLoading = false;
        }

        private async Task EditRowAsync(DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystem)
        {
            await grid.EditRow(dailyProducedWaterTreatmentSystem);
        }

        private async Task SaveRowAsync(DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystem)
        {
            try
            {
                if (dailyProducedWaterTreatmentSystem == dailyProducedWaterTreatmentSystemToInsert)
                {
                    dailyProducedWaterTreatmentSystemToInsert = null;
                }
                using var serviceScope = ServiceProvider.CreateScope();
                var dailyProducedWaterTreatmentSystemService = this.GetGenericService(serviceScope);
                await dailyProducedWaterTreatmentSystemService.UpdateAsync(dailyProducedWaterTreatmentSystem, dailyProducedWaterTreatmentSystem.Id);
                await grid.UpdateRow(dailyProducedWaterTreatmentSystem);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        private void CancelEdit(DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystem)
        {
            if (dailyProducedWaterTreatmentSystem == dailyProducedWaterTreatmentSystemToInsert)
            {
                dailyProducedWaterTreatmentSystemToInsert = null;
            }

            grid.CancelEditRow(dailyProducedWaterTreatmentSystem);

        }

        private async Task DeleteRowAsync(DailyProducedWaterTreatmentSystem dailyProducedWaterTreatmentSystem)
        {
            try
            {
                if (dailyProducedWaterTreatmentSystem == dailyProducedWaterTreatmentSystemToInsert)
                {
                    dailyProducedWaterTreatmentSystemToInsert = null;
                }

                if (dailyProducedWaterTreatmentSystems.Contains(dailyProducedWaterTreatmentSystem))
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var dailyProducedWaterTreatmentSystemService = this.GetGenericService(serviceScope);
                    await dailyProducedWaterTreatmentSystemService.DeleteAsync(dailyProducedWaterTreatmentSystem);
                    await grid.Reload();
                }
                else
                {
                    grid.CancelEditRow(dailyProducedWaterTreatmentSystem);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        private void HandleException(Exception ex)
        {
            NotificationService.Notify(new()
            {
                Summary = "Error",
                Detail = ex.InnerException?.ToString(),
                Severity = NotificationSeverity.Error,
                Duration = 120000,
            });
        }

        private IGenericService<DailyProducedWaterTreatmentSystem> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyProducedWaterTreatmentSystem, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task OnChangeAsync(object value, string name)
        {
            search = value.ToString();
            AppendQuery();
            await this.ReloadAsync();
        }
        private async Task OnChangeStatusFilterAsync(object value, string name)
        {
            status = value?.ToString();
            AppendQuery();
            await this.ReloadAsync();
        }

        private void OnTodayClick()
        {
            date = DateTime.Now;
        }

        private async Task OnChangeAsync(DateTime? value, string name, string format)
        {
            date = value;
            AppendQuery();
            await this.ReloadAsync();
        }

        private void AppendQuery()
        {
            var queries = new Dictionary<string, string>();
            if (search != null)
            {
                queries.Add(nameof(search), search);
            }
            if (status != null)
            {
                queries.Add(nameof(status), status);
            }
            if (date != null)
            {
                queries.Add(nameof(date), date.Value.ToString("yyyy-MM-dd"));
            }
            var uriBuilder = new UriBuilder(NavManager.Uri);
            NavManager.NavigateTo(QueryHelpers.AddQueryString(uriBuilder.Uri.AbsolutePath, queries));
        }
    }
}
