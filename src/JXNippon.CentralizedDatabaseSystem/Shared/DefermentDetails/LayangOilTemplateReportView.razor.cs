using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class LayangOilTemplateReportView
    {
        private bool isCollapsed = false;
        private Menu menu;
        private LayangOilTemplateReport Items { get; set; }
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
        private IGenericService<LayangOilTemplateReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<LayangOilTemplateReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        public async Task LoadDataAsync(int? selectedYear = null)
        {
            using var serviceScopeUser = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScopeUser);

            if (selectedYear is null)
            {
                Items = (await service.Get()
                    .Where(x => x.Year == DateTime.Now.Year)
                    .ToQueryOperationResponseAsync<LayangOilTemplateReport>())
                    .FirstOrDefault();
            }
            else
            {
                Items = (await service.Get()
                    .Where(x => x.Year == selectedYear)
                    .ToQueryOperationResponseAsync<LayangOilTemplateReport>())
                    .FirstOrDefault();
            }

            StateHasChanged();
        }
        public async Task LoadYearAsync(int? selectedYear = null)
        {
            using var serviceScopeUser = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScopeUser);

            YearList = (await service.Get()
                .ToQueryOperationResponseAsync<LayangOilTemplateReport>())
                .Select(x => x.Year)
                .OrderDescending()
                .ToList();

            StateHasChanged();
        }
        public async Task OnDataUpdateHandler(string year)
        {
            await this.LoadDataAsync(Convert.ToInt16(year));
        }
    }
}
