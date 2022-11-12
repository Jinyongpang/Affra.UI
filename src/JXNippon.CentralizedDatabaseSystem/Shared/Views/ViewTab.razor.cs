using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewTab
    {
        [Parameter] public string Page { get; set; }
        [Parameter] public DateTime? Date { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }


        private IEnumerable<View> views = new List<View>();
        private View view = new View();

        private ViewComponent viewComponent;

        private bool hasFocus { get; set; }

        private string selectedTabKey;


        protected override async Task OnInitializedAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            views = await viewService.GetPageViewsAsync(Page);
            if (views.Any())
            {
                await OnTabClickAsync(views.FirstOrDefault().Name);
            }
        }

        public async Task OnTabClickAsync(string key)
        {
            selectedTabKey = key;
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            view = await viewService.GetViewAsync(key);
            StateHasChanged();
            await viewComponent.ReloadAsync();
        }
    }
}
