using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class MonthFilterComponent : IDateFilterComponent
    {
        [Parameter] public string Title { get; set; }
        public DateTime? Start => this.dateRange.Start;

        public DateTime? End => this.dateRange.End;
        public event OnDateRangeChangedHandler OnDateRangeChanged;

        private DateRange dateRange = new DateRange();

        private DatePicker<DateTime?> datePicker;

        public Task OnRangeSelectAsync(DateTimeChangedEventArgs args)
        {
            this.CalculateDateRange(args.Date);

            return OnDateRangeChanged is null ? Task.CompletedTask : OnDateRangeChanged(dateRange);
        }

        protected override Task OnInitializedAsync()
        {
            this.CalculateDateRange(DateTime.Now.Date);
            return Task.CompletedTask;
        }

        private void CalculateDateRange(DateTime month)
        {
            this.dateRange.Start = new DateTime(month.Year, month.Month, 1);
            this.dateRange.End = this.dateRange.Start.Value.AddMonths(1).AddDays(-1);
        }
    }
}