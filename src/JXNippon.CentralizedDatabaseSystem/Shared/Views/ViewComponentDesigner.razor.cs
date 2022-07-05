using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewComponentDesigner
    {
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<View> views = new List<View>();
        private RadzenDropDown<View> radzenDropDown;
        private View view = new View();
        private ViewComponent viewComponent;
        private ColumnDataGrid columnDataGrid;
        private RowDataGrid rowDataGrid;

        protected override async Task OnInitializedAsync()
        {
            views = (await GetViewsAsync()).ToList();
        }

        protected async Task<IEnumerable<View>> GetViewsAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            return await viewService.GetViewsAndRowsAsync();

        }

        private async Task OnChangeAsync(object value)
        {
            view = value as View;
            await this.ReloadAsync();
        }

        public async Task ReloadAsync(View value = null)
        {
            view = value ?? this.view;
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            await viewService.GetViewDetailAsync(view);
            await viewComponent.ReloadAsync();
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            if (view != null
                && !string.IsNullOrEmpty(view.Name))
            {
                using var serviceScope = ServiceProvider.CreateScope();
                IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
                views = (await GetViewsAsync()).ToList();
                radzenDropDown.SelectedItem = views
                    .Where(x => x.Name == view?.Name)
                    .FirstOrDefault();
                view = radzenDropDown.SelectedItem as View;
                await viewService.GetViewDetailAsync(view);
                await viewComponent.ReloadAsync();
            }
        }

        private async Task AddRowAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var genericService = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<Row, IViewUnitOfWork>>();
            var seq = view.Rows.Count > 0
                ? view.Rows.Max(row => row.Sequence) + 1
                : 0;
            var row = new Row() { ViewName = view.Name, Sequence = seq };
            await genericService.InsertAsync(row);
            AffraNotificationService.NotifyItemCreated();
            view.Rows.Add(row);
            StateHasChanged();
        }

        private IGenericService<T> GetGenericService<T>(IServiceScope serviceScope)
            where T : class
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<T, IViewUnitOfWork>>();
        }

        private async Task ShowDialogAsync(Column data, string title)
        {
            dynamic? response = null;
            if (data.ComponentType == nameof(Chart))
            {
                response = await DialogService.OpenAsync<ChartDialog>(title,
                   new Dictionary<string, object>() { { "Column", data }, { "View", view } },
                   Constant.DialogOptions);
            }
            else if (data.ComponentType == nameof(Grid))
            {
                response = await DialogService.OpenAsync<GridDialog>(title,
                   new Dictionary<string, object>() { { "Column", data }, { "View", view } },
                   Constant.DialogOptions);
            }


            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService<Column>(serviceScope);

                    if (data.Id > 0)
                    {
                        await service.UpdateAsync(data, data.Id);
                        AffraNotificationService.NotifyItemUpdated();
                    }
                    else
                    {
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
                    await this.ReloadAsync();
                }
            }
        }
    }
}
