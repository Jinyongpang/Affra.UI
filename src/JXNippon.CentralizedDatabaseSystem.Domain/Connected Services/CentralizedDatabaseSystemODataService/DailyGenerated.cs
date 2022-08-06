using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Helpers;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using Microsoft.OData.Client;

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments 
{ 
    public partial class DailyHealthSafetyEnvironment : IDaily
    { 
        [IgnoreClientProperty]
        public DateTime DateUI 
        { 
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? LastLTIDateUI
        {
            get { return this.LastLTIDate.ToLocalDateTime(); }
            set { this.LastLTIDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? LastDrillExerciseConductedDateUI
        {
            get { return this.LastDrillExerciseConducted.ToLocalDateTime(); }
            set { this.LastDrillExerciseConducted = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? LastAnnualESDTestDateUI
        {
            get { return this.LastAnnualESDTest.ToLocalDateTime(); }
            set { this.LastAnnualESDTest = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? NextAnnualESDTestDateUI
        {
            get { return this.NextAnnualESDTest.ToLocalDateTime(); }
            set { this.NextAnnualESDTest = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? Injurious_MTC_FACDateUI
        {
            get { return this.Injurious_MTC_FACDate.ToLocalDateTime(); }
            set { this.Injurious_MTC_FACDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? NearMissDateUI
        {
            get { return this.NearMissDate.ToLocalDateTime(); }
            set { this.NearMissDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? PropertyDamageDateUI
        {
            get { return this.PropertyDamageDate.ToLocalDateTime(); }
            set { this.PropertyDamageDate = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyLifeBoat : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments 
{ 
    public partial class DailyLongTermOverridesInhibitsOnAlarmTrip : IDaily 
    { 
        [IgnoreClientProperty]
        public DateTime DateUI 
        { 
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime RaisedDateUI
        {
            get { return this.RaisedDate.ToLocalDateTime(); }
            set { this.RaisedDate = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyLossOfPrimaryContainmentIncident : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments 
{
    public partial class DailyOperatingChange : IDaily
    { 
        [IgnoreClientProperty] 
        public DateTime DateUI 
        { 
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime RaisedDateUI
        {
            get { return this.RaisedDate.ToLocalDateTime(); }
            set { this.RaisedDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? ExpiredDateUI
        {
            get { return this.ExpiredDate?.ToLocalDateTime(); }
            set { this.ExpiredDate = value?.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries 
{ 
    public partial class DailyHIPAndLWPSummary : IDaily 
    { 
        [IgnoreClientProperty] public DateTime DateUI 
        { 
            get { return this.Date.ToLocalDateTime(); } 
            set { this.Date = value.ToUniversalTime(); } 
        } 

        [IgnoreClientProperty]
        public IDictionary<string, object>? ExtraDictionary
        {
            get 
            {
                return this.Extras.ToExtrasObject();
            }
        }
    } 
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries { public partial class DailyFPSOHelangSummary : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SandDisposalDesanders 
{ 
    public partial class DailySandDisposalDesander : IDaily 
    { 
        [IgnoreClientProperty] 
        public DateTime DateUI 
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime LastSandJettingDateUI
        {
            get { return this.LastSandJettingDate.ToLocalDateTime(); }
            set { this.LastSandJettingDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime NextSandJettingDateUI
        {
            get { return this.NextSandJettingDate.ToLocalDateTime(); }
            set { this.NextSandJettingDate = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ChemicalInjections 
{ 
    public partial class DailyCiNalco : IDaily 
    { 
        [IgnoreClientProperty] public DateTime DateUI 
        { 
            get { return this.Date.ToLocalDateTime(); } 
            set { this.Date = value.ToUniversalTime(); } 
        }

        [IgnoreClientProperty]
        public IDictionary<string, object>? ExtraDictionary
        {
            get
            {
                return this.Extras.ToExtrasObject();
            }
        }
    } 
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ChemicalInjections { public partial class DailyInowacInjection : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CommunicationSystems { public partial class DailyCommunicationSystem : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.LWPActivities { public partial class DailyLWPActivity : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Vendors { public partial class DailyVendorActivity : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyUtilityBase : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyUtility : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyWaterTank : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyNitrogenGenerator : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MaximoWorkOrders { public partial class DailyMaximoWorkOrder : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems { public partial class DailyAnalysisResult : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems { public partial class DailyCoolingMediumSystem : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Logistics { public partial class DailyLogistic : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolPump : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolTrain : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolStock : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.RollsRoyceGasEngineAndKawasakiCompressionSystems { public partial class DailyKawasakiExportCompressor : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.RollsRoyceGasEngineAndKawasakiCompressionSystems
{
    public partial class DailyRollsRoyceRB211Engine : IDaily
    {
        [IgnoreClientProperty] 
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); } 
        }

        [IgnoreClientProperty]
        public DateTime TurbineWashDateUI
        {
            get { return this.TurbineWashDate.ToLocalDateTime(); }
            set { this.TurbineWashDate = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeads { public partial class DailyHIPWellHeadParameter : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeads { public partial class DailyLWPWellHeadParameter : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GasCondensateExportSamplerAndExportLines { public partial class DailyGasCondensateExportSamplerAndExportLine : IDaily 
    { 
        [IgnoreClientProperty]
        public DateTime DateUI 
        { 
            get { return this.Date.ToLocalDateTime(); } 
            set { this.Date = value.ToUniversalTime(); } 
        }

        [IgnoreClientProperty]
        public DateTime? LastPiggingDateUI
        {
            get { return this.LastPiggingDate.ToLocalDateTime(); }
            set { this.LastPiggingDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? NextPiggingDateUI
        {
            get { return this.NextPiggingDate.ToLocalDateTime(); }
            set { this.NextPiggingDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? LastChangeOutDateUI
        {
            get { return this.LastChangeOut.ToLocalDateTime(); }
            set { this.LastChangeOut = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? NextChangeOutDateUI
        {
            get { return this.NextChangeOut.ToLocalDateTime(); }
            set { this.NextChangeOut = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? LastSamplingDateUI
        {
            get { return this.LastSampling.ToLocalDateTime(); }
            set { this.LastSampling = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? NextSamplingDateUI
        {
            get { return this.NextSampling.ToLocalDateTime(); }
            set { this.NextSampling = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems { public partial class DailyWellHeadAndSeparationSystem : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems { public partial class DailyWellStreamCooler : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailySK10Production : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailyHIPProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailyFPSOHelangProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MajorEquipmentStatuses { public partial class DailyMajorEquipmentStatus : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems 
{ 
    public partial class DailyProducedWaterTreatmentSystem : IDaily 
    { 
        [IgnoreClientProperty] 
        public DateTime DateUI 
        {
            get { return this.Date.ToLocalDateTime(); } 
            set { this.Date = value.ToUniversalTime(); } 
        }

        [IgnoreClientProperty]
        public DateTime LastSkimmingDateUI
        {
            get { return this.LastSkimmingDate.ToLocalDateTime(); }
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
            get { return this.Date.ToLocalDateTime(); } 
            set { this.Date = value.ToUniversalTime(); } 
        } 
    } 
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PowerGenerationAndDistributions { public partial class DailyPowerGenerationAndDistribution : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }