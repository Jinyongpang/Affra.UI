using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeads;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.WellHead
{
    public partial class HIPWellHeadParameterDialog : IDailyDialog<DailyHIPWellHeadParameter>
    {
        private IEnumerable<HIPWellHead> datas;
        [Parameter] public DailyHIPWellHeadParameter Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected override async Task OnInitializedAsync()
        {
            if (!isViewing)
            {
                using var serviceScope = ServiceProvider.CreateScope();
                datas = (await serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<HIPWellHead, ICentralizedDatabaseSystemUnitOfWork>>()
                    .Get()
                    .ToQueryOperationResponseAsync<HIPWellHead>()).ToList();
            }
        }
    }
}
