using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Commons
{
    public partial class CollectionView<TItem>
        where TItem : class
    {

        [Parameter]
        public RenderFragment<TItem>? ChildContent { get; set; }

        [Parameter]
        public ICollection<TItem> Items { get; set; }

        [Parameter]
        public bool ShowEmpty { get; set; } = true;

        public Task ReloadAsync()
        {
            this.StateHasChanged();
            return Task.CompletedTask;
        }
    }
}
