using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems
{
    public partial class DailyWellStreamCooler : IDaily
    {
        public DateTime DateUI 
        { 
            get 
            { 
                return this.Date.LocalDateTime;            
            }
            set 
            { 
                this.Date = value.ToUniversalTime(); 
            } 
        }
    }
}
