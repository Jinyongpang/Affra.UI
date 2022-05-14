using Affra.Core.Domain.Extensions;
using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class DailyProductionFilterPanel
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
            var sk = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<SK10ProductionItem, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<SK10ProductionItem>();
            var hip = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<HIPProductionItem, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<HIPProductionItem>();
            var fpso = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<FPSOHelangProductionItem, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<FPSOHelangProductionItem>();

            List<string> names = (await sk)
                .Select(item => item.Name)
                .ToList();

            names.AddRange((await hip)
                .Select(item => item.Name)
                .ToList());

            names.AddRange((await fpso)
                .Select(item => item.Name)
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
