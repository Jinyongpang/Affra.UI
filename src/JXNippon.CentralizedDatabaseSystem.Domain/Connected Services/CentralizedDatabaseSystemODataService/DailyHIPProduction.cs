using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions
{
    public partial class DailyHIPProduction : IDaily
    {
        public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } }
    }
}
