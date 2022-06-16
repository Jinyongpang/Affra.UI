using Microsoft.AspNetCore.Components;
using Radzen;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GasCondensateExportSamplerAndExportLines;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Affra.Core.Infrastructure.OData.Extensions;

namespace JXNippon.CentralizedDatabaseSystem.Shared.GasAndCondensateExportSamplersAndExportLine
{
    public partial class GasCondensateExportSamplerAndExportLineDialog
    {
        private IEnumerable<DailyGasCondensateExportSamplerAndExportLine> datas;
        [Parameter] public DailyGasCondensateExportSamplerAndExportLine Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected override async Task OnInitializedAsync()
        {
            if (!isViewing)
            {
                using var serviceScope = ServiceProvider.CreateScope();
                datas = (await serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyGasCondensateExportSamplerAndExportLine, ICentralizedDatabaseSystemUnitOfWork>>()
                    .Get()
                    .ToQueryOperationResponseAsync<DailyGasCondensateExportSamplerAndExportLine>()).ToList();
            }
        }

        protected Task SubmitAsync(DailyGasCondensateExportSamplerAndExportLine arg)
        {
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
