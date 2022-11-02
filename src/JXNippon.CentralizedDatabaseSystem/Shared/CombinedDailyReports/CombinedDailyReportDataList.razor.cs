﻿using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.OData.Client;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportDataList
    {
        private int count;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
        private Virtualize<CombinedDailyReport> virtualize;

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IUserService UserService { get; set; }

        public CommonFilter Filter { get; set; }

        protected override Task OnInitializedAsync()
        {
            Filter = new CommonFilter(navigationManager);
            return Task.CompletedTask;
        }
        public async Task ReloadAsync()
        {
            await virtualize.RefreshDataAsync();
            this.StateHasChanged();
        }

        private async ValueTask<ItemsProviderResult<CombinedDailyReport>> LoadDataAsync(ItemsProviderRequest request)
        {
            isLoading = true;
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.CombinedDailyReport), HasReadPermissoin = true, HasWritePermission = true });
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<CombinedDailyReport>? combinedDailyReportService = GetGenericService(serviceScope);
                var query = combinedDailyReportService.Get();
                if (Filter.Status != null)
                {
                    var status = (CombinedDailyReportStatus)Enum.Parse(typeof(CombinedDailyReportStatus), Filter.Status);
                    query = query.Where(CombinedDailyReport => CombinedDailyReport.Status == status);
                }
                if (Filter.DateRange?.Start != null)
                {
                    var start = Filter.DateRange.Start.ToUniversalTime();
                    var end = Filter.DateRange.End.ToUniversalTime();
                    query = query
                        .Where(CombinedDailyReport => CombinedDailyReport.Date >= start)
                        .Where(CombinedDailyReport => CombinedDailyReport.Date <= end);
                }

                Microsoft.OData.Client.QueryOperationResponse<CombinedDailyReport>? combinedDailyReportsResponse = await query
                    .OrderByDescending(combinedDailyReport => combinedDailyReport.Date)
                    .Skip(request.StartIndex)
                    .Take(request.Count)
                    .ToQueryOperationResponseAsync<CombinedDailyReport>();

                count = (int)combinedDailyReportsResponse.Count;
                var combinedDailyReportsList = combinedDailyReportsResponse.ToList();

                return new ItemsProviderResult<CombinedDailyReport>(combinedDailyReportsList, count);
            }
            finally
            {
                initLoading = false;
                isLoading = false;
                StateHasChanged();
            }
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<CombinedDailyReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<CombinedDailyReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private async Task ShowDialogAsync(CombinedDailyReport data)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericService(serviceScope);
            var query = (DataServiceQuery<CombinedDailyReport>)service.Get();

            var response = await query
                .Expand(x => x.DailyHealthSafetyEnvironment)
                .Expand(x => x.DailyLifeBoats)
                .Expand(x => x.DailyLongTermOverridesInhibitsOnAlarmTrips)
                .Expand(x => x.DailyLossOfPrimaryContainmentIncident)
                .Expand(x => x.DailyOperatingChanges)
                .Expand(x => x.DailyHIPAndLWPSummarys)
                .Expand(x => x.DailyFPSOHelangSummarys)
                .Expand(x => x.DailySandDisposalDesander)
                .Expand(x => x.DailyCiNalco)
                .Expand(x => x.DailyInowacInjection)
                .Expand(x => x.DailyCommunicationSystems)
                .Expand(x => x.DailyLWPActivitys)
                .Expand(x => x.DailyVendorActivitys)
                .Expand(x => x.DailyUtilitys)
                .Expand(x => x.DailyWaterTank)
                .Expand(x => x.DailyNitrogenGenerator)
                .Expand(x => x.DailyMaximoWorkOrders)
                .Expand(x => x.DailyAnalysisResult)
                .Expand(x => x.DailyCoolingMediumSystems)
                .Expand(x => x.DailyLogistics)
                .Expand(x => x.DailyGlycolPumps)
                .Expand(x => x.DailyGlycolTrains)
                .Expand(x => x.DailyGlycolStock)
                .Expand(x => x.DailyKawasakiExportCompressors)
                .Expand(x => x.DailyRollsRoyceRB211Engines)
                .Expand(x => x.DailyHIPWellHeadParameters)
                .Expand(x => x.DailyLWPWellHeadParameters)
                .Expand(x => x.DailyGasCondensateExportSamplerAndExportLine)
                .Expand(x => x.DailyWellHeadAndSeparationSystem)
                .Expand(x => x.DailyWellStreamCoolers)
                .Expand(x => x.DailySK10Production)
                .Expand(x => x.DailyHIPProduction)
                .Expand(x => x.DailyFPSOHelangProduction)
                .Expand(x => x.DailyMajorEquipmentStatuses)
                .Expand(x => x.DailyDiesel)
                .Expand(x => x.DailyProducedWaterTreatmentSystems)
                .Expand(x => x.DailyDeOilerInjection)
                .Expand(x => x.DailyPowerGenerationAndDistributions)
                .Where(x => x.Date == data.Date)
                .ToQueryOperationResponseAsync<CombinedDailyReport>();

            var cdrItem = response.FirstOrDefault();

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

            dynamic? dialogResponse;
            dialogResponse = await DialogService.OpenAsync<CombinedDailyReportView>(data.Date.ToLocalTime().ToString("d"),
                       new Dictionary<string, object>() { { "Data", cdrItem } },
                       Constant.FullScreenDialogOptions);

            if (dialogResponse == true)
            {

            }

            await this.ReloadAsync();
        }
    }
}
