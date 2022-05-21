using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeads;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class WellHeadFilterPanel
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

            var lwp = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyLWPWellHeadParameter, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<DailyLWPWellHeadParameter>();
            var hip = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyHIPWellHeadParameter, ICentralizedDatabaseSystemUnitOfWork>>()
                .Get()
                .ToQueryOperationResponseAsync<DailyHIPWellHeadParameter>();

            List<string> names = (await lwp)
                .Select(x => x.LWPWellHeadName)
                .ToList();
            names.AddRange((await hip)
                .Select(x => x.HIPWellHeadName)
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
