namespace JXNippon.CentralizedDatabaseSystem.Domain.Filters
{
    public delegate Task OnDateRangeChangedHandler(DateRange dateRange);
    public interface IDateFilterComponent
    {
        DateTime? Start { get; }

        DateTime? End { get; }


        event OnDateRangeChangedHandler OnDateRangeChanged;
    }
}
