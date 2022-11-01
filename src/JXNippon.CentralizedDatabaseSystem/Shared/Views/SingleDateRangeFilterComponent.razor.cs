using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class SingleDateRangeFilterComponent : IDateFilterComponent
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public string Class { get; set; }
        [Parameter] public bool IsGlobal { get; set; }
        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }
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
            this.CalculateDateRange(IsGlobal
                ? DateTime.Now.Date
                : this.GlobalDataSource.GlobalDateFilter.Start.Value.Date);

            return Task.CompletedTask;
        }

        private void CalculateDateRange(DateTime? datetime)
        {
            this.dateRange.Start = datetime;
            this.dateRange.End = this.dateRange.Start.Value.AddDays(1).AddMilliseconds(-1);
        }
    }
}