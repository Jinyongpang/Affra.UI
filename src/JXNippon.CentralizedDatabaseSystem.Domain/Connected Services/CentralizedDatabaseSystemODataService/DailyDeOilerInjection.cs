using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems
{
    public partial class DailyDeOilerInjection : IDaily
    {
        public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } }
    }
}
