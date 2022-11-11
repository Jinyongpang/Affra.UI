using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.PEMonthlyReports
{
    public class PEMonthlyReportService : IPEMonthlyReportService
    {
        private readonly IUnitGenericService<PEReport, ICentralizedDatabaseSystemUnitOfWork> service;

        public PEMonthlyReportService(IUnitGenericService<PEReport, ICentralizedDatabaseSystemUnitOfWork> service)
        {
            this.service = service;
        }

        public async Task<PEReport> GetPEMonthlyReportAsync(DateTimeOffset date)
        {
            var query = (DataServiceQuery<PEReport>)service.Get();
            var response = await ((DataServiceQuery<PEReport>)query
                .Expand(x => x.DailyHIPSales)
                .Expand(x => x.DailyFPSOSales)
                .Expand(x => x.DailyGasMeterings)
                .Expand(x => x.DailyCondensateCalculateds)
                .Expand(x => x.MonthlyHIPSale)
                .Expand(x => x.MonthlyFPSOSale)
                .Expand(x => x.DailyHIPFieldDs)
                .Expand(x => x.DailyFPSOFieldDs)
                .Expand(x => x.MonthlyHIPFieldMY)
                .Expand(x => x.MonthlyFPSOFieldMY)
                .Expand(x => x.MonthlyReservoirs)
                .Expand(x => x.MonthlyReservoirProductions)
                .Expand(x => x.MonthlyWellProductions)
                .Expand(x => x.MonthlyWellTests)
                .Expand(x => x.DailyEstimatedWellGasProductions)
                .Expand(x => x.DailyAllocatedWellGasProductions)
                .Expand(x => x.DailyEstimatedWellCondensateProductions)
                .Expand(x => x.DailyAllocatedWellCondensateProductions)
                .Expand(x => x.DailyEstimatedWellWaterProductions)
                .Expand(x => x.DailyAllocatedWellWaterProductions)
                .Expand(x => x.DailyHL1WellProductionCalculations)
                .Expand(x => x.DailyHL2WellProductionCalculations)
                .Expand(x => x.DailyHL3WellProductionCalculations)
                .Expand(x => x.DailyHL4WellProductionCalculations)
                .Expand(x => x.DailyHL5WellProductionCalculations)
                .Expand(x => x.DailyHL6WellProductionCalculations)
                .Expand(x => x.DailyHL7WellProductionCalculations)
                .Expand(x => x.DailyHL8WellProductionCalculations)
                .Expand(x => x.DailyHL9WellProductionCalculations)
                .Expand(x => x.DailyHL10WellProductionCalculations)
                .Expand(x => x.DailyHL11WellProductionCalculations)
                .Expand(x => x.DailyHL12WellProductionCalculations)
                .Expand(x => x.DailyHM1WellProductionCalculations)
                .Expand(x => x.DailyHM2WellProductionCalculations)
                .Expand(x => x.DailyHM3WellProductionCalculations)
                .Expand(x => x.DailyHM4WellProductionCalculations)
                .Expand(x => x.DailyHM5WellProductionCalculations)
                .Expand(x => x.DailyLA1WellProductionCalculations)
                .Expand(x => x.DailyLA2WellProductionCalculations)
                .Expand(x => x.DailyLA3WellProductionCalculations)
                .Expand(x => x.DailyBU1ST1WellProductionCalculations)
                .Expand(x => x.DailyBU2WellProductionCalculations)
                .Expand(x => x.DailyBU3WellProductionCalculations)
                .Where(x => x.Date == date))
                .ExecuteAsync();

            var perItem = response.FirstOrDefault();

            if (perItem is null)
            {
                return null;
            }

            perItem.MonthlyHIPSale ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SalesMY.MonthlyHIPSale
            {
                Date = perItem.Date,
                Month = perItem.Date
            };
            perItem.MonthlyFPSOSale ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SalesMY.MonthlyFPSOSale
            {
                Date = perItem.Date,
                Month = perItem.Date
            };
            perItem.MonthlyHIPFieldMY ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.FieldMY.MonthlyHIPFieldMY
            {
                Date = perItem.Date,
                Month = perItem.Date
            };
            perItem.MonthlyFPSOFieldMY ??= new CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.FieldMY.MonthlyFPSOFieldMY
            {
                Date = perItem.Date,
                Month = perItem.Date
            };

            return perItem;
        }
    }
}
