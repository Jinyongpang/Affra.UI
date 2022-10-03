using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class DateRangeFilterComponent : IDateFilterComponent
    {
        [Parameter] public string Title { get; set; }
        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }
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
            dateRange.End = this.GlobalDataSource.GlobalDateFilter.Start.Value;
            dateRange.Start = dateRange.End.Value.AddDays(days);
            rangePicker.Value = new DateTime?[] { dateRange.Start.Value, dateRange.End.Value };
            rangePicker.Close();
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