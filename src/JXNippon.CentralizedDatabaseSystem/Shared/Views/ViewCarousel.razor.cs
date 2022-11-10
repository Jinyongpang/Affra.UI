using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Views;
using static System.Net.WebRequestMethods;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewCarousel : IDisposable
    {
        private Timer timer;
        private bool isDisposed;
        private int currentIndex = 0;
        private View view = new View();
        private ViewComponent viewComponent;

        [Parameter] public string Page { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }


        private ICollection<View> views = new List<View>();

        private bool hasFocus { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            views = (await viewService.GetPageViewsAsync(Page)).ToList();
            this.StateHasChanged();
            if (views.Any())
            {
                await this.RefreshViewAsync();
            }
            timer = new Timer(async (object? stateInfo) =>
            {
                this.ChangeView();
                await this.RefreshViewAsync();
                StateHasChanged();
            }, new AutoResetEvent(false), 60000, 60000); 
        }

        public void Dispose()
        {
            if (!isDisposed)
            { 
                this.timer?.Dispose();
            }
        }

        private void ChangeView()
        {
            this.currentIndex++;
            if (currentIndex == views.Count)
            {
                this.currentIndex = 0;
            }
        }
           
        private async Task RefreshViewAsync()
        {
            string currentView = this.views.ElementAt(this.currentIndex).Name;
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            view = await viewService.GetViewAsync(currentView);
            StateHasChanged();
            await this.viewComponent.ReloadAsync();
        }
    }
}
