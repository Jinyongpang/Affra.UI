﻿using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.LivingQuartersUtilitiesAndOthers
{
    public partial class WaterTankDataGrid
    {
        private RadzenDataGrid<DailyWaterTank> grid;
        private IEnumerable<DailyWaterTank> items;
        private bool isLoading = false;
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public EventCallback Refresh { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        public Task ReloadAsync()
        {
            return Task.WhenAll(grid.FirstPage(true), Refresh.InvokeAsync());
        }
        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            await LoadData.InvokeAsync();
            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScope);
            var query = service.Get();
            if (CommonFilter != null)
            {
                if (!string.IsNullOrEmpty(CommonFilter.Search))
                {
                    query = query.Where(x => x.UtilityName.ToUpper().Contains(CommonFilter.Search.ToUpper()));
                }
                if (CommonFilter.Status != null)
                {
                    query = query.Where(x => x.Status.ToUpper() == CommonFilter.Status.ToUpper());
                }
                if (CommonFilter.Date != null)
                {
                    var start = TimeZoneInfo.ConvertTimeToUtc(CommonFilter.Date.Value);
                    var end = TimeZoneInfo.ConvertTimeToUtc(CommonFilter.Date.Value.AddDays(1));
                    query = query
                        .Where(x => x.Date >= start)
                        .Where(x => x.Date < end);
                }
            }

            var response = await query
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DailyWaterTank>();

            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<DailyWaterTank> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyWaterTank, ICentralizedDatabaseSystemUnitOfWork>>();
        }
    }
}