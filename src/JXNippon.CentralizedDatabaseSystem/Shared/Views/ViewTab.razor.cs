using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewTab
    {
        [Parameter] public string Page { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }


        private IEnumerable<View> views = new List<View>();
        private View view = new View();

        private ViewComponent viewComponent;

        private bool hasFocus { get; set; }

        private string selectedTabKey;

        private ICollection<ICollection<string>> colorsGroups = new List<ICollection<string>>();

        protected override async Task OnInitializedAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            views = await viewService.GetPageViewsAsync(this.Page);
            if (views.Any())
            {
                await this.OnTabClickAsync(views.FirstOrDefault().Name);
            }
        }

        public async Task OnTabClickAsync(string key)
        {
            this.selectedTabKey = key;
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            view = await viewService.GetViewAsync(key);
            colorsGroups = new List<ICollection<string>>();
            foreach (var row in view.Rows)
                foreach(var col in row.Columns)
                {
                    colorsGroups.Add(Constants.Constant.GetRandomColors());
                }

            StateHasChanged();
            await this.viewComponent.ReloadAsync();      
        }
    }
}
