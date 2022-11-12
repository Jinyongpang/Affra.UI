using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Filters
{
    public delegate Task OnDateRangeChangedHandler(DateRange dateRange);
    public interface IDateFilterComponent
    {
        DateTime? FixDateTime { get; set; }

        DateTime? Start { get; }

        DateTime? End { get; }

        IGlobalDataSource GlobalDataSource { get; set; }

        event OnDateRangeChangedHandler OnDateRangeChanged;

        DateTime? InitialDateTime 
        { 
            get 
            {
                return this.FixDateTime ?? GlobalDataSource.GlobalDateFilter.Start;
            } 
        }

    }
}
