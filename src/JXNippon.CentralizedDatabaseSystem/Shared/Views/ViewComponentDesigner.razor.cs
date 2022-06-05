using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewComponentDesigner
    {
        [Inject] private IServiceProvider ServiceProvider { get; set; }

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
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            await viewService.GetViewDetailAsync(view);
            await Task.WhenAll(columnDataGrid.ReloadAsync(), rowDataGrid.ReloadAsync());
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
    }
}
