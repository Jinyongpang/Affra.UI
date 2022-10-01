using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class HorizontalMonthFilterComponent : IDateFilterComponent
    {
        [Parameter] public string Title { get; set; }

        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }

        public DateTime? Start => this.dateRange.Start;

        public DateTime? End => this.dateRange.End;
        public event OnDateRangeChangedHandler OnDateRangeChanged;

        private DateRange dateRange = new DateRange();

        private ICollection<int> years = new List<int>();

        private string[] months = new string[]
        {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec",
        };

        private int month;
        private int year;

        protected override Task OnInitializedAsync()
        {
            for (int i = 2022; i < 2050; i++)
            {
                years.Add(i);
            }
            this.month = this.GlobalDataSource.GlobalDateFilter.Start.Value.Month;
            this.year = this.GlobalDataSource.GlobalDateFilter.Start.Value.Year;
            this.CalculateDateRange();
            return Task.CompletedTask;
        }

        private void CalculateDateRange()
        {
            this.dateRange.Start = new DateTime(this.year, this.month, 1);
            this.dateRange.End = this.dateRange.Start.Value.AddMonths(1).AddDays(-1);
        }

        private Task OnChangeAsync()
        {
            this.CalculateDateRange();
            return OnDateRangeChanged is null ? Task.CompletedTask : OnDateRangeChanged(dateRange);
        }

    }
}