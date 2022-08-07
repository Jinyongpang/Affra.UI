﻿using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Domain.TemplateManagements;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Commons
{
    public partial class DailyDataGrid<TItem, TDialog>
        where TItem : class, IDaily, IExtras, IEntity
        where TDialog : ComponentBase
    {
        private RadzenDataGrid<TItem> grid;
        private IEnumerable<TItem> items;
        private bool isLoading = false;
        private IEnumerable<CustomColumn> customColumns;

        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool ShowRefreshButton { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool ShowDateColumn { get; set; }
        [Parameter] public RenderFragment Columns { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        [Inject] private IExtraColumnService ExtraColumnService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            customColumns = (await serviceScope.ServiceProvider.GetRequiredService<IExtraColumnService>()
                .GetCustomColumns(typeof(TItem).Name)
                .ToQueryOperationResponseAsync<CustomColumn>())
                .ToList();
        }
        public async Task ReloadAsync()
        {
            await grid.FirstPage(true);
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
                if (CommonFilter.Date != null)
                {
                    var start = TimeZoneInfo.ConvertTimeToUtc(CommonFilter.Date.Value);
                    var end = TimeZoneInfo.ConvertTimeToUtc(CommonFilter.Date.Value.AddDays(1));
                    query = query
                        .Cast<IDaily>()
                        .Where(x => x.Date >= start)
                        .Where(x => x.Date < end)
                        .Cast<TItem>();
                }
            }

            var response = await query
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<TItem>();

            Count = (int)response.Count;
            items = response.ToList();
            isLoading = false;
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<TItem> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task ShowDialogAsync(TItem data, int menuAction, string title)
        {
            ContextMenuService.Close();
            dynamic? response;
            if (menuAction == 2)
            {
                response = await DialogService.OpenAsync<GenericConfirmationDialog>(title,
                           new Dictionary<string, object>() { },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

                if (response == true)
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService(serviceScope);
                    await service.DeleteAsync(data);

                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<TDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction } },
                           Constant.DialogOptions);

                if (response == true)
                {
                    try
                    {
                        using var serviceScope = ServiceProvider.CreateScope();
                        var service = this.GetGenericService(serviceScope);

                        if (data.AsIEntity().Id > 0)
                        {
                            isLoading = true;
                            await service.UpdateAsync(data, data.AsIEntity().Id);
                            AffraNotificationService.NotifyItemUpdated();
                        }
                        else
                        {
                            isLoading = true;
                            await service.InsertAsync(data);
                            AffraNotificationService.NotifyItemCreated();
                        }

                    }
                    catch (Exception ex)
                    {
                        AffraNotificationService.NotifyException(ex);
                    }
                    finally
                    {
                        isLoading = false;
                    }
                }
            }

            await grid.Reload();
        }
    }
}
