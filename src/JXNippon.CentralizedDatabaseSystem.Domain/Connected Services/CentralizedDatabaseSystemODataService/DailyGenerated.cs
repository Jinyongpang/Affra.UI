using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using Microsoft.OData.Client;

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyHealthSafetyEnvironment : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyLifeBoat : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyLongTermOverridesInhibitsOnAlarmTrip : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyLossOfPrimaryContainmentIncident : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyOperatingChange : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries { public partial class DailyHIPAndLWPSummary : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries { public partial class DailyFPSOHelangSummary : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SandDisposalDesanders 
{ 
    public partial class DailySandDisposalDesander : IDaily 
    { 
        [IgnoreClientProperty] 
        public DateTime DateUI 
        {
            get { return this.Date.LocalDateTime; }
            set { this.Date = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime LastSandJettingDateUI
        {
            get { return this.LastSandJettingDate.LocalDateTime; }
            set { this.LastSandJettingDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime NextSandJettingDateUI
        {
            get { return this.NextSandJettingDate.LocalDateTime; }
            set { this.NextSandJettingDate = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ChemicalInjections { public partial class DailyCiNalco : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ChemicalInjections { public partial class DailyInowacInjection : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CommunicationSystems { public partial class DailyCommunicationSystem : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.LWPActivities { public partial class DailyLWPActivity : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Vendors { public partial class DailyVendorActivity : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyUtilityBase : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyUtility : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyWaterTank : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyNitrogenGenerator : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MaximoWorkOrders { public partial class DailyMaximoWorkOrder : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems { public partial class DailyAnalysisResult : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems { public partial class DailyCoolingMediumSystem : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Logistics { public partial class DailyLogistic : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolPump : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolTrain : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolStock : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.RollsRoyceGasEngineAndKawasakiCompressionSystems { public partial class DailyKawasakiExportCompressor : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.RollsRoyceGasEngineAndKawasakiCompressionSystems { public partial class DailyRollsRoyceRB211Engine : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeads { public partial class DailyHIPWellHeadParameter : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeads { public partial class DailyLWPWellHeadParameter : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GasCondensateExportSamplerAndExportLines { public partial class DailyGasCondensateExportSamplerAndExportLine : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems { public partial class DailyWellHeadAndSeparationSystem : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems { public partial class DailyWellStreamCooler : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailySK10Production : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailyHIPProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailyFPSOHelangProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MajorEquipmentStatuses { public partial class DailyMajorEquipmentStatus : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems 
{ 
    public partial class DailyProducedWaterTreatmentSystem : IDaily 
    { 
        [IgnoreClientProperty] 
        public DateTime DateUI 
        {
            get { return this.Date.LocalDateTime; } 
            set { this.Date = value.ToUniversalTime(); } 
        }

        [IgnoreClientProperty]
        public DateTime LastSkimmingDateUI
        {
            get { return this.LastSkimmingDate.LocalDateTime; }
            set { this.LastSkimmingDate = value.ToUniversalTime(); }
        }
    } 
}

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems 
{ 
    public partial class DailyDeOilerInjection : IDaily 
    { 
        [IgnoreClientProperty] 
        public DateTime DateUI 
        { 
            get { return this.Date.LocalDateTime; } 
            set { this.Date = value.ToUniversalTime(); } 
        } 
    } 
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PowerGenerationAndDistributions { public partial class DailyPowerGenerationAndDistribution : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }