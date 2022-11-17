using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Domain.TemplateManagements;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.AuditTrails;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Commons
{
    public partial class DailyDataGrid<TItem, TDialog>
        where TItem : class, IDaily, IExtras, IEntity
        where TDialog : ComponentBase, IDailyDialog<TItem>
    {
        private RadzenDataGrid<TItem> grid;

        private bool isLoading = false;
        private IEnumerable<CustomColumn> customColumns;
        private Collection<TItem> _items { get; set; }
        [Parameter] public Collection<TItem> Items { get; set; }
        [Parameter] public EventCallback<Collection<TItem>> ItemsChanged { get; set; }
        [Parameter] public EventCallback<Collection<TItem>> OnItemsChanged { get; set; }
        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Parameter] public bool PagerAlwaysVisible { get; set; }
        [Parameter] public bool AllowSorting { get; set; } = true;
        [Parameter] public bool AllowFiltering { get; set; } = false;
        [Parameter] public bool RequiredHighlighting { get; set; } = true;
        [Parameter] public RenderFragment Columns { get; set; }
        [Parameter] public EventCallback<IQueryable<TItem>> QueryFilter { get; set; }

        [Parameter] public string[] Subscriptions { get; set; } = new string[0];
        [Parameter] public DateTimeOffset? ReportDate { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }
        [Inject] private IExtraColumnService ExtraColumnService { get; set; }
        [Inject] private IContentUpdateNotificationService ContentUpdateNotificationService { get; set; }
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }
        private bool isDisposed = false;

        protected override async Task OnInitializedAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            customColumns = (await serviceScope.ServiceProvider.GetRequiredService<IExtraColumnService>()
                .GetCustomColumns(typeof(TItem).Name)
                .ToQueryOperationResponseAsync<CustomColumn>())
                .ToList();
            foreach (var subscription in Subscriptions)
            {
                await ContentUpdateNotificationService.SubscribeAsync(subscription, OnContentUpdateAsync);
            }

            await ContentUpdateNotificationService.SubscribeAsync(typeof(TItem).Name, OnContentUpdateAsync);
        }
        public async Task ReloadAsync()
        {
            await grid.FirstPage(true);
        }
        private Task OnContentUpdateAsync(object obj)
        {
            StateHasChanged();
            return ReloadAsync();
        }
        protected async virtual Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            await LoadData.InvokeAsync();
            using var serviceScope = ServiceProvider.CreateScope();
            var service = GetGenericService(serviceScope);
            var query = service.Get();
            if (CommonFilter != null)
            {
                if (CommonFilter.Date != null)
                {
                    var start = TimeZoneInfo.ConvertTimeToUtc(CommonFilter.Date.Value);
                    var end = TimeZoneInfo.ConvertTimeToUtc(CommonFilter.Date.Value.AddDays(1));
                    query = query
                        .Where(x => x.Date >= start)
                        .Where(x => x.Date < end);
                }
            }
            await QueryFilter.InvokeAsync(query);

            var response = await query
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<TItem>();

            Count = (int)response.Count; 
            Items = new Collection<TItem>(response.ToArray());
            var itemCountBefore = Items.Count;
            await ItemsChanged.InvokeAsync(Items);
            await OnItemsChanged.InvokeAsync(Items);
            var itemCountAfter = Items.Count;
            Count = itemCountAfter - itemCountBefore + Count;
            _items = Items;
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
                    var service = GetGenericService(serviceScope);
                    await service.DeleteAsync(data);

                    AffraNotificationService.NotifyItemDeleted();
                }
            }
            else if (menuAction == 5)
            {
                _ = await DialogService.OpenAsync<AuditTrailTable>(title,
                           new Dictionary<string, object>() { ["Id"] = data.Id, ["TableName"] = typeof(TItem).Name },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });
                return;
            }
            else if (menuAction == 6)
            {
                _ = await DialogService.OpenAsync<AuditTrailTable>(title,
                           new Dictionary<string, object>() { ["Action"] = CentralizedDatabaseSystemODataService.Affra.Core.Domain.AuditTrails.Action.Delete, ["TableName"] = typeof(TItem).Name },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });
                return;
            }
            else
            {
                response = await DialogService.OpenAsync<TDialog>(title,
                           new Dictionary<string, object>() { { "Item", data }, { "MenuAction", menuAction }, { "CustomColumns", customColumns } },
                           Constant.DialogOptions);

                if (response == true)
                {
                    try
                    {
                        using var serviceScope = ServiceProvider.CreateScope();
                        var service = GetGenericService(serviceScope);

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
        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!isDisposed)
                {
                    foreach (var subscription in Subscriptions)
                    {
                        await ContentUpdateNotificationService.RemoveHandlerAsync(subscription, OnContentUpdateAsync);
                    }
                    await ContentUpdateNotificationService.RemoveHandlerAsync(typeof(TItem).Name, OnContentUpdateAsync);

                    isDisposed = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void CellRender(DataGridCellRenderEventArgs<TItem> args)
        {
            if (!args.Column.Property.Contains("Remark")
                && !args.Column.Property.Equals("NTD")
				&& string.IsNullOrEmpty(args.Column.GetValue(args.Data) as string))
            {
                args.Attributes.Add("style", "background-color: #FFFF99");
            }
        }
        public object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
