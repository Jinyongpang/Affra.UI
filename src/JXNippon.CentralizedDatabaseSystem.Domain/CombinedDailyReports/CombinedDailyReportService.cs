using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.CombinedDailyReports
{
    public class CombinedDailyReportService : ICombinedDailyReportService
    {
        private readonly IUnitGenericService<CombinedDailyReport, ICentralizedDatabaseSystemUnitOfWork> service;

        public CombinedDailyReportService(IUnitGenericService<CombinedDailyReport, ICentralizedDatabaseSystemUnitOfWork> service)
        {
            this.service = service;
        }

        public async Task<CombinedDailyReport> GetCombinedDailyReportAsync(DateTimeOffset date)
        {
            var query = (DataServiceQuery<CombinedDailyReport>)service.Get();
            var response = await ((DataServiceQuery<CombinedDailyReport>)query
                .Expand(x => x.DailyHealthSafetyEnvironment)
                .Expand(x => x.DailyLossOfPrimaryContainmentIncident)
                .Expand(x => x.DailySandDisposalDesander)
                .Expand(x => x.DailyCiNalco)
                .Expand(x => x.DailyInowacInjection)
                .Expand(x => x.DailyWaterTank)
                .Expand(x => x.DailyNitrogenGenerator)
                .Expand(x => x.DailyAnalysisResult)
                .Expand(x => x.DailyGlycolStock)
                .Expand(x => x.DailyGasCondensateExportSamplerAndExportLine)
                .Expand(x => x.DailyWellHeadAndSeparationSystem)
                .Expand(x => x.DailySK10Production)
                .Expand(x => x.DailyHIPProduction)
                .Expand(x => x.DailyFPSOHelangProduction)
                .Expand(x => x.DailyDiesel)
                .Expand(x => x.DailyDeOilerInjection)
                .Where(x => x.Date == date))
                .ExecuteAsync();

            var cdrItem = response.FirstOrDefault();

            if (cdrItem is null)
            {
                return null;
            }

            cdrItem.DailyHealthSafetyEnvironment ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments.DailyHealthSafetyEnvironment
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyLossOfPrimaryContainmentIncident ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments.DailyLossOfPrimaryContainmentIncident
            {
                Date = cdrItem.Date
            };
            cdrItem.DailySK10Production ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions.DailySK10Production
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyHIPProduction ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions.DailyHIPProduction
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyFPSOHelangProduction ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.DailyProductions.DailyFPSOHelangProduction
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyDiesel ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.MajorEquipmentStatuses.DailyDiesel
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyCiNalco ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ChemicalInjections.DailyCiNalco
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyInowacInjection ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ChemicalInjections.DailyInowacInjection
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyWellHeadAndSeparationSystem ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellHeadAndSeparationSystems.DailyWellHeadAndSeparationSystem
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyGlycolStock ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GlycolRegenerationSystems.DailyGlycolStock
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyGasCondensateExportSamplerAndExportLine ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.GasCondensateExportSamplerAndExportLines.DailyGasCondensateExportSamplerAndExportLine
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyAnalysisResult ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CoolingMediumSystems.DailyAnalysisResult
            {
                Date = cdrItem.Date
            };
            cdrItem.DailyDeOilerInjection ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.ProducedWaterTreatmentSystems.DailyDeOilerInjection
            {
                Date = cdrItem.Date
            };
            cdrItem.DailySandDisposalDesander ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SandDisposalDesanders.DailySandDisposalDesander
            {
                SandDisposalDesanderName = "Sand Jetting Desander",
                Date = cdrItem.Date
            };
            cdrItem.DailyNitrogenGenerator ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities.DailyNitrogenGenerator
            {
                UtilityName = "Nitrogen Generator, A-5900",
                Date = cdrItem.Date
            };
            cdrItem.DailyWaterTank ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Utilities.DailyWaterTank
            {
                UtilityName = "Potable Water Tank, T-5250",
                Date = cdrItem.Date
            };

            return cdrItem;
        }
    }
}
