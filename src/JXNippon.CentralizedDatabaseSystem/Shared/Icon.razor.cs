using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class Icon
    {
        private ElementReference icon;

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string Content { get; set; }

        [Parameter]
        public TooltipOptions TooltipOptions { get; set; }

        [Parameter]
        public bool HideText { get; set; }

        [Inject]
        private TooltipService TooltipService { get; set; }

        private string? NavItemLabelCssClass => HideText ? "sideBarItemLabelHide" : "sideBarItemLabelShow";

        private void ShowTooltip(ElementReference elementReference)
        {
            if (HideText)
            {
                TooltipService.Open(elementReference, Content, TooltipOptions);
            }
        }
    }
}
