﻿using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.GlycolRegenerationSystem
{
    public partial class GlycolStockDialog
    {
        private IEnumerable<DailyGlycolStock> datas;
        [Parameter] public DailyGlycolStock Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private bool isViewing { get => MenuAction == 3; }

        protected override async Task OnInitializedAsync()
        {
            if (!isViewing)
            {
                using var serviceScope = ServiceProvider.CreateScope();
                datas = (await serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyGlycolStock, ICentralizedDatabaseSystemUnitOfWork>>()
                    .Get()
                    .ToQueryOperationResponseAsync<DailyGlycolStock>()).ToList();
            }
        }

        protected Task SubmitAsync(DailyGlycolStock arg)
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