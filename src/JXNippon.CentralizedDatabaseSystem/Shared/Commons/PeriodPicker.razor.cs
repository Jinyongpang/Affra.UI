using AntDesign;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Commons
{
    public partial class PeriodPicker
    {
        [Parameter]
        public DateTime? StartDate { get; set; } = DateTime.Now.Date.AddDays(-30);

        [Parameter]
        public EventCallback<DateTime?> StartDateChanged { get; set; }

        [Parameter]
        public DateTime? EndDate { get; set; } = DateTime.Now.Date;

        [Parameter]
        public EventCallback<DateTime?> EndDateChanged { get; set; }

        [Parameter]
        public EventCallback<Period> OnChanged { get; set; }


        private RangePicker<DateTime?[]> rangePicker;

        public async Task OnRangeSelectAsync(DateRangeChangedEventArgs args)
        {
            StartDate = args.Dates[0];
            await StartDateChanged.InvokeAsync(StartDate);
            EndDate = args.Dates[1];
            await EndDateChanged.InvokeAsync(EndDate);
            await OnChanged.InvokeAsync(new() { StartDate = this.StartDate, EndDate = this.EndDate });
        }

        private async Task SetDateAsync(int days)
        {
            StartDate = DateTime.Now.Date;
            await StartDateChanged.InvokeAsync(StartDate);
            EndDate = EndDate.Value.AddDays(days);

            await EndDateChanged.InvokeAsync(EndDate);
            rangePicker.Value = new DateTime?[] { StartDate, EndDate };
            rangePicker.Close();
            await OnChanged.InvokeAsync(new() { StartDate = this.StartDate, EndDate = this.EndDate });
        }
    }

    public class Period
    { 
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
