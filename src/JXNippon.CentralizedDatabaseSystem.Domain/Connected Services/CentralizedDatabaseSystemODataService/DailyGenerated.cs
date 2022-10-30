using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Helpers;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using Microsoft.OData.Client;

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments
{
    public partial class DailyHealthSafetyEnvironment : IDaily, IExtras, IEntity
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
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyLifeBoat : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments
{
    public partial class DailyLongTermOverridesInhibitsOnAlarmTrip : IDaily, IExtras, IEntity
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
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments { public partial class DailyLossOfPrimaryContainmentIncident : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments
{
    public partial class DailyOperatingChange : IDaily, IExtras, IEntity
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
    public partial class DailyHIPAndLWPSummary : IDaily, IExtras, IEntity
    {
        [IgnoreClientProperty]
        public DateTime DateUI
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
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries { public partial class DailyFPSOHelangSummary : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SandDisposalDesanders
{
    public partial class DailySandDisposalDesander : IDaily, IExtras, IEntity
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? LastSandJettingDateUI
        {
            get { return this.LastSandJettingDate.ToLocalDateTime(); }
            set { this.LastSandJettingDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? NextSandJettingDateUI
        {
            get { return this.NextSandJettingDate.ToLocalDateTime(); }
            set { this.NextSandJettingDate = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ChemicalInjections
{
    public partial class DailyCiNalco : IDaily, IExtras, IEntity
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ChemicalInjections { public partial class DailyInowacInjection : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CommunicationSystems { public partial class DailyCommunicationSystem : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.LWPActivities { public partial class DailyLWPActivity : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Vendors { public partial class DailyVendorActivity : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyUtilityBase : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyUtility : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyWaterTank : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities { public partial class DailyNitrogenGenerator : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MaximoWorkOrders { public partial class DailyMaximoWorkOrder : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems { public partial class DailyAnalysisResult : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems { public partial class DailyCoolingMediumSystem : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Logistics { public partial class DailyLogistic : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolPump : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolTrain : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems { public partial class DailyGlycolStock : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.RollsRoyceGasEngineAndKawasakiCompressionSystems { public partial class DailyKawasakiExportCompressor : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.RollsRoyceGasEngineAndKawasakiCompressionSystems
{
    public partial class DailyRollsRoyceRB211Engine : IDaily, IExtras, IEntity
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
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeads { public partial class DailyHIPWellHeadParameter : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeads { public partial class DailyLWPWellHeadParameter : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GasCondensateExportSamplerAndExportLines
{
    public partial class DailyGasCondensateExportSamplerAndExportLine : IDaily, IExtras, IEntity
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
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems { public partial class DailyWellHeadAndSeparationSystem : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems { public partial class DailyWellStreamCooler : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailySK10Production : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailyHIPProduction : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions { public partial class DailyFPSOHelangProduction : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MajorEquipmentStatuses { public partial class DailyMajorEquipmentStatus : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MajorEquipmentStatuses { public partial class DailyDiesel : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems
{
    public partial class DailyProducedWaterTreatmentSystem : IDaily, IExtras, IEntity
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
    public partial class DailyDeOilerInjection : IDaily, IExtras, IEntity
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PowerGenerationAndDistributions { public partial class DailyPowerGenerationAndDistribution : IDaily, IExtras, IEntity { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.ToLocalDateTime(); } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SalesD { public partial class DailyHIPSale : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SalesD { public partial class DailyFPSOSale : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SalesD { public partial class DailyGasMetering : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SalesD { public partial class DailyCondensateCalculated : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL1WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL2WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL3WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL4WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL5WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL6WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL7WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL8WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL9WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL10WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL11WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHL12WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHM1WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHM2WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHM3WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHM4WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyHM5WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyLA1WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyLA2WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyLA3WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyBU1ST1WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyBU2WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations { public partial class DailyBU3WellProductionCalculation : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellWaterProductions { public partial class DailyEstimatedWellWaterProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellWaterProductions { public partial class DailyAllocatedWellWaterProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellGasProductions { public partial class DailyEstimatedWellGasProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellGasProductions { public partial class DailyAllocatedWellGasProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellCondensateProductions { public partial class DailyEstimatedWellCondensateProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellCondensateProductions { public partial class DailyAllocatedWellCondensateProduction : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.FieldD { public partial class DailyFPSOFieldD : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.FieldD { public partial class DailyHIPFieldD : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments { public partial class DailyGasProductionDeliverySchedule : IDaily { [IgnoreClientProperty] public DateTime DateUI { get { return this.Date.LocalDateTime; } set { this.Date = value.ToUniversalTime(); } } } }

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments
{
    public partial class DefermentDetail
    {
        [IgnoreClientProperty]
        public DateTime? StartDateUI
        {
            get { return this.StartDate.ToLocalDateTime(); }
            set { this.StartDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime? EndDateUI
        {
            get { return this.EndDate.ToLocalDateTime(); }
            set { this.EndDate = value.ToUniversalTime(); }
        }
    }
}

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductionCalculations
{
    public partial class WellCoefficient
    {
        [IgnoreClientProperty]
        public DateTime? LastUpdatedDateUI
        {
            get { return this.LastUpdatedDate.ToLocalDateTime(); }
            set { this.LastUpdatedDate = value.ToUniversalTime(); }
        }
    }
}