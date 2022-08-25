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


        private DateTimeOffset? startDate { get; set; } = DateTime.Now.Date.AddDays(-30);
        private DateTimeOffset? endDate { get; set; } = DateTime.Now.Date;

        private IEnumerable<View> views = new List<View>();
        private View view = new View();

        private ViewComponent viewComponent;

        private RangePicker<DateTime?[]> rangePicker;

        private bool hasFocus { get; set; }

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

        public Task OnRangeSelectAsync(DateRangeChangedEventArgs args)
        {
            startDate = args.Dates[0];
            endDate = args.Dates[1];
            return viewComponent.ReloadAsync(startDate, endDate);
        }

        private Task SetDateAsync(int days)
        {
            endDate = DateTime.Now.Date;
            startDate = endDate.Value.AddDays(days);
            rangePicker.Value = new DateTime?[] { startDate.Value.DateTime, endDate.Value.DateTime };
            rangePicker.Close();
            return viewComponent.ReloadAsync(startDate, endDate);
        }

        public async Task OnTabClickAsync(string key)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            view = await viewService.GetViewAsync(key);
            StateHasChanged();
        }
    }
}
