using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class YearFilterComponent : IDateFilterComponent
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public DateTime? FixDateTime { get; set; }
        [Parameter] public EventCallback<DateRange> OnChanged { get; set; }

        [Inject] public IGlobalDataSource GlobalDataSource { get; set; }
        public DateTime? Start => this.dateRange.Start;

        public DateTime? End => this.dateRange.End;
        public event OnDateRangeChangedHandler OnDateRangeChanged;

        private DateRange dateRange = new DateRange();

        private DatePicker<DateTime?> datePicker;

        public Task OnRangeSelectAsync(DateTimeChangedEventArgs args)
        {
            this.CalculateDateRange(args.Date);
            OnChanged.InvokeAsync(dateRange);
            return OnDateRangeChanged is null ? Task.CompletedTask : OnDateRangeChanged(dateRange);
        }

        protected override Task OnInitializedAsync()
        {
            this.CalculateDateRange(this.AsIDateFilterComponent().InitialDateTime.Value.Date);
            return Task.CompletedTask;
        }

        private void CalculateDateRange(DateTime? year)
        {
            this.dateRange.Start = new DateTime(year.Value.Year, 1, 1);
            this.dateRange.End = this.dateRange.Start.Value.AddYears(1).AddDays(-1);
        }
    }
}