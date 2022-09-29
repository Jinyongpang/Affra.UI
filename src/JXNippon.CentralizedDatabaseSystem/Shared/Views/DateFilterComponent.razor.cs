using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class DateFilterComponent
    {
        [Parameter] public DateFilter DateFilter { get; set; }

        [Parameter] public Column Column { get; set; }

        public IDateFilterComponent Filter { get; set; }

        protected override Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

    }
}