using System.Collections.ObjectModel;
using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.OIMSummaries;
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

        public Collection<DailyHIPAndLWPSummary> AppendSummary(Collection<DailyHIPAndLWPSummary> data, CombinedDailyReport combinedDailyReport)
        {
            var result = new List<DailyHIPAndLWPSummary>();
            if (combinedDailyReport is not null
                && combinedDailyReport.DailySK10Production is not null
                && combinedDailyReport.DailySK10Production.SK10CumulativeGasExport is not null)
            {
                result.Add(new()
                {
                    Date = combinedDailyReport.Date,
                    Remark = this.CalculateHIPSummary(combinedDailyReport),
                });

                result.Add(new()
                {
                    Date = combinedDailyReport.Date,
                    Remark = combinedDailyReport.DailySK10Production.SK10CumulativeGasExport > 0
                        ? "Refer to section 18 below for details on vendor activities reports."
                        : "Refer to shutdown daily progress report",
                });
            }
            foreach (var item in data)
            {
                result.Add(item);
            }
            data = new Collection<DailyHIPAndLWPSummary>(result);
            return data;
        }
        public Collection<DailyFPSOHelangSummary> AppendSummary(Collection<DailyFPSOHelangSummary> data, CombinedDailyReport combinedDailyReport)
        {
            var result = new List<DailyFPSOHelangSummary>();
            if (combinedDailyReport is not null
                && combinedDailyReport.DailyFPSOHelangProduction is not null
                && combinedDailyReport.DailyFPSOHelangProduction.FPSOCumulativeGasExport is not null)
            {
                result.Add(new()
                {
                    Date = combinedDailyReport.Date,
                    Remark = this.CalculateFPSOSummary(combinedDailyReport),
                });
            }
            foreach (var item in data)
            {
                result.Add(item);
            }
            data = new Collection<DailyFPSOHelangSummary>(result);
            return data;
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

        private string CalculateFPSOSummary(CombinedDailyReport combinedDailyReport)
        {
            var fpsoProduction = combinedDailyReport.DailyFPSOHelangProduction;
            if (fpsoProduction.FPSOCumulativeGasExport > 0)
            {
                return $"FPSO Helang cumulative Gas Export of "
                    + $"{fpsoProduction.FPSOCumulativeGasExport}"
                    + " MMscf was "
                    + $"{this.GetFPSOCondition(combinedDailyReport)}"
                    + " than GODC "
                    + $"{combinedDailyReport.DailyFPSOHelangProduction?.FPSOCumulativeGasExportRemark?.Trim()}"
                    + " plan of "
                    + $"{combinedDailyReport.DailyFPSOHelangProduction?.FPSOGasExportGODCPlan}"
                    + " MMscf. FPSO daily cumulative Crude Oil into storage tank was "
                    + $"{combinedDailyReport.DailyFPSOHelangProduction?.FPSOCumulativeCrudeOil}"
                    + " bbls.";
            }
            else if (fpsoProduction.FPSOCumulativeGasExport == 0)
            {
                return "FPSO Helang shutdown";
            }
            return string.Empty;
        }

        private string GetFPSOCondition(CombinedDailyReport combinedDailyReport)
        {
            var fpsoGasExport = combinedDailyReport.DailyFPSOHelangProduction?.FPSOCumulativeGasExport ?? 0;
            var fpsoGasExportPlan = combinedDailyReport.DailyFPSOHelangProduction?.FPSOGasExportGODCPlan ?? 0;
            var difference = fpsoGasExport - fpsoGasExportPlan;
            if (difference < 0 && difference > -1.0m)
            {
                return "Slightly lower";
            }
            else if (difference < 0)
            {
                return "lower";
            }
            else if (difference > 0 && difference < 1.0m)
            {
                return "Slightly higher";
            }
            else if (difference > 0)
            {
                return "higher";
            }
            return "Equal";
        }
        private string CalculateHIPSummary(CombinedDailyReport combinedDailyReport)
        {
            var sk10Production = combinedDailyReport.DailySK10Production;
            if (sk10Production.SK10CumulativeGasExport > 0)
            {
                return "SK-10 cumulative Gas Export of "
                    + $"{sk10Production.SK10CumulativeGasExport}"
                    + $" MMscf (HIP: "
                    + $"{combinedDailyReport.DailyHIPProduction?.HIPCumulativeGasExport ?? 0}"
                    + " MMscf, FPSO Helang: "
                    + $"{combinedDailyReport.DailyFPSOHelangProduction?.FPSOCumulativeGasExport ?? 0}"
                    + " MMscf) was "
                    + $"{this.GetHIPCondition(combinedDailyReport)}"
                    + " than SK-10 GODC "
                    + $"{combinedDailyReport.DailyHIPProduction?.HIPCumulativeGasExportRemark?.Trim()}"
                    + " plan of "
                    + $"{combinedDailyReport.DailyHIPProduction?.HIPGasExportGODCPlan}"
                    + " MMscfd";
            }
            else if (sk10Production.SK10CumulativeGasExport == 0)
            {
                return "SK-10 Helang remain zero production";
            }
            return string.Empty;
        }

        private string GetHIPCondition(CombinedDailyReport combinedDailyReport)
        {
            var sk10GasExport = combinedDailyReport.DailySK10Production?.SK10CumulativeGasExport ?? 0;
            var hipGasExporPlan = combinedDailyReport.DailyHIPProduction?.HIPGasExportGODCPlan ?? 0;
            var difference = sk10GasExport - hipGasExporPlan;
            if (difference < 0 && difference > -1.0m)
            {
                return "Slightly lower";
            }
            else if (difference < 0)
            {
                return "lower";
            }
            else if (difference > 0 && difference < 1.0m)
            {
                return "Slightly higher";
            }
            else if (difference > 0)
            {
                return "higher";
            }
            return "Equal";
        }

    }
}
