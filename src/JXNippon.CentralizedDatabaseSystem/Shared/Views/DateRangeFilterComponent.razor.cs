using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class DateRangeFilterComponent : IDateFilterComponent
    {
        [Parameter] public string Title { get; set; }
        public DateTime? Start => this.dateRange.Start;

        public DateTime? End => this.dateRange.End;

        private RangePicker<DateTime?[]> rangePicker;

        private DateRange dateRange = new DateRange();

        public event OnDateRangeChangedHandler OnDateRangeChanged;

        public Task OnRangeSelectAsync(DateRangeChangedEventArgs args)
        {
            dateRange.Start = args.Dates[0];
            dateRange.End = args.Dates[1];

            return OnDateRangeChanged is null ? Task.CompletedTask : OnDateRangeChanged(dateRange);
        }

        private Task SetDateAsync(int days)
        {
            dateRange.End = DateTime.Now.Date;
            dateRange.Start = dateRange.End.Value.AddDays(days);
            rangePicker.Value = new DateTime?[] { dateRange.Start.Value, dateRange.End.Value };
            rangePicker.Close();
            return OnDateRangeChanged is null ? Task.CompletedTask : OnDateRangeChanged(dateRange);
        }

        protected override Task OnInitializedAsync()
        {
            dateRange.Start = DateTime.Now.Date.AddDays(-30);
            dateRange.End =  DateTime.Now.Date;
            return Task.CompletedTask;
        }

    }
}