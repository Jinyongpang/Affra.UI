using BlazorDateRangePicker;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Shared.Views;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Dashboard
    {
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        private DateTimeOffset? startDate { get; set; } = DateTime.Now.Date.AddDays(-7);
        private DateTimeOffset? endDate { get; set; } = DateTime.Now.Date;

        private View view = new View();

        private ViewComponent viewComponent;

        protected override async Task OnInitializedAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            view = await viewService.GetViewAsync(nameof(Dashboard));
        }

        public async Task OnRangeSelectAsync(DateRange range)
        {
            await viewComponent.ReloadAsync(range.Start, range.End);
        }
    }
}
