using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class LastXPeriodFilterComponent : IDateFilterComponent
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public DateTime? FixDateTime { get; set; }

        [Inject] public IGlobalDataSource GlobalDataSource { get; set; }
        public DateTime? Start => this.dateRange.Start;

        public DateTime? End => this.dateRange.End;
        public event OnDateRangeChangedHandler OnDateRangeChanged;

        private DateRange dateRange = new DateRange();

        private int unit = 1;
        private int number = 7;

        protected override Task OnInitializedAsync()
        {
            this.CalculateDateRange();
            return Task.CompletedTask;
        }

        private void CalculateDateRange()
        {
            this.dateRange.End = this.AsIDateFilterComponent().InitialDateTime.Value.Date;

            if (this.unit == 1)
            {
                this.dateRange.Start = this.dateRange.End.Value.AddDays(-number);
            }
            else if (this.unit == 2)
            {
                this.dateRange.Start = this.dateRange.End.Value.AddMonths(-number);
            }
            else if (this.unit == 3)
            {
                this.dateRange.Start = this.dateRange.End.Value.AddYears(-number);
            }
        }

        private Task OnChangeAsync()
        {
            this.CalculateDateRange();
            return OnDateRangeChanged is null ? Task.CompletedTask : OnDateRangeChanged(dateRange);
        }
    }
}