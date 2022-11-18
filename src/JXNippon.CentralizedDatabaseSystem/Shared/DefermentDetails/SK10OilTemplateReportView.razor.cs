using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class SK10OilTemplateReportView
    {
        private bool isCollapsed = false;
        private Menu menu;
        private SK10OilTemplateReport Items { get; set; }
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
        private IGenericService<SK10OilTemplateReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<SK10OilTemplateReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }
        public async Task LoadDataAsync(int? selectedYear = null)
        {
            using var serviceScopeUser = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScopeUser);

            if (selectedYear is null)
            {
                Items = (await service.Get()
                    .Where(x => x.Year == DateTime.Now.Year)
                    .ToQueryOperationResponseAsync<SK10OilTemplateReport>())
                    .FirstOrDefault();
            }
            else
            {
                Items = (await service.Get()
                    .Where(x => x.Year == selectedYear)
                    .ToQueryOperationResponseAsync<SK10OilTemplateReport>())
                    .FirstOrDefault();
            }

            StateHasChanged();
        }
        public async Task LoadYearAsync(int? selectedYear = null)
        {
            using var serviceScopeUser = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScopeUser);

            YearList = (await service.Get()
                .ToQueryOperationResponseAsync<SK10OilTemplateReport>())
                .Select(x => x.Year)
                .OrderDescending()
                .ToList();

            StateHasChanged();
        }
    }
}
