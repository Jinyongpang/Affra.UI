namespace JXNippon.CentralizedDatabaseSystem.Models
{
    public class Series
    {
        public string Title { get; set; }

        public IEnumerable<SeriesItem> SeriesItems{ get; set; }
    }
}
