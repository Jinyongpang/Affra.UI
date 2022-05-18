using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class WellHeadAndSeparationSystemFilterPanel
    {
        private IEnumerable<DailyWellHeadAndSeparationSystem> datas;

        [Parameter] public EventCallback<CommonFilter> Change { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        public CommonFilter CommonFilter { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager);

            using var serviceScope = ServiceProvider.CreateScope();
            datas = (await serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyWellHeadAndSeparationSystem, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<DailyWellHeadAndSeparationSystem>()).ToList();
        }
        private Task OnChangeAsync(object value)
        {
            CommonFilter.AppendQuery(NavManager);
            return Change.InvokeAsync(CommonFilter);
        }
    }
}
