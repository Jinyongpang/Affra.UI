using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.AspNetCore.Components;
using Radzen;
using SandDisposalDesanders = CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SandDisposalDesanders;

namespace JXNippon.CentralizedDatabaseSystem.Shared.SandDisposalDesander
{
    public partial class SandDisposalDesanderDialog
    {
        private IEnumerable<SandDisposalDesanders.SandDisposalDesander> datas;
        [Parameter] public SandDisposalDesanders.DailySandDisposalDesander Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected override async Task OnInitializedAsync()
        {
            if (!isViewing)
            {
                using var serviceScope = ServiceProvider.CreateScope();
                datas = (await serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<SandDisposalDesanders.SandDisposalDesander, ICentralizedDatabaseSystemUnitOfWork>>()
                    .Get()
                    .ToQueryOperationResponseAsync<SandDisposalDesanders.SandDisposalDesander>()).ToList();
            }
        }

        protected Task SubmitAsync(SandDisposalDesanders.DailySandDisposalDesander arg)
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
