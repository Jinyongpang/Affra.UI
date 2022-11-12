using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class DateRangeFilterComponent : IDateFilterComponent
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public DateTime? FixDateTime { get; set; }

        [Parameter] public EventCallback<DateRange> OnChanged { get; set; }

        [Inject] public IGlobalDataSource GlobalDataSource { get; set; }
        public DateTime? Start => this.dateRange.Start;

        public DateTime? End => this.dateRange.End;

        public event OnDateRangeChangedHandler OnDateRangeChanged;

        private RangePicker<DateTime?[]> rangePicker;

        private DateRange dateRange = new DateRange();


        public Task OnRangeSelectAsync(DateRangeChangedEventArgs args)
        {
            dateRange.Start = args.Dates[0];
            dateRange.End = args.Dates[1];

            OnChanged.InvokeAsync(dateRange);
            return OnDateRangeChanged is null ? Task.CompletedTask : OnDateRangeChanged(dateRange);
        }

        private Task SetDateAsync(int days)
        {
            dateRange.End = this.GlobalDataSource.GlobalDateFilter.Start.Value;
            dateRange.Start = dateRange.End.Value.AddDays(days);
            rangePicker.Value = new DateTime?[] { dateRange.Start.Value, dateRange.End.Value };
            rangePicker.Close();
            OnChanged.InvokeAsync(dateRange);
            return OnDateRangeChanged is null ? Task.CompletedTask : OnDateRangeChanged(dateRange);
        }

        protected override Task OnInitializedAsync()
        {
            dateRange.Start = this.GlobalDataSource.GlobalDateFilter.Start.Value.AddDays(-30);
            dateRange.End = this.GlobalDataSource.GlobalDateFilter.Start.Value.Date;
            return Task.CompletedTask;
        }

    }
}