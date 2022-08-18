using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Shared.TemplateManagement;
using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Commons
{
    public partial class DailyDialog<TItem>
        where TItem : class, IExtras
    {
        private ExtraColumnEditor extraColumnEditor;
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }
        [Parameter] public TItem Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool IsViewing { get; set; }
        [Inject] private DialogService DialogService { get; set; }


        protected Task SubmitAsync(TItem arg)
        {
            arg.Extras = extraColumnEditor.GetExtras();
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
