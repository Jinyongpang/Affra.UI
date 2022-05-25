using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.LivingQuartersUtilitiesAndOthers
{
    public partial class LivingQuartersUtilitiesAndOtherFilterPanel
    {
        private IEnumerable<NameFilter> datas;
        [Parameter] public EventCallback<CommonFilter> Change { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        public CommonFilter CommonFilter { get; set; }
        protected override async Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager);

            using var serviceScope = ServiceProvider.CreateScope();

            var utilities = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyUtility, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<DailyUtility>();
            var nitrogenGenerator = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyNitrogenGenerator, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<DailyNitrogenGenerator>();
            var waterTank = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyWaterTank, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<DailyWaterTank>();

            List<string> names = (await utilities)
                .Select(x => x.UtilityName)
                .ToList();
            names.AddRange((await nitrogenGenerator)
                .Select(x => x.UtilityName)
                .ToList());
            names.AddRange((await waterTank)
                .Select(x => x.UtilityName)
                .ToList());

            datas = names.Distinct()
                .Select(name => new NameFilter(name))
                .ToList();

        }
        private async Task OnChangeAsync(object value)
        {
            CommonFilter.AppendQuery(NavManager);
            await Change.InvokeAsync(CommonFilter);
        }
    }
}
