using Affra.Core.Domain.Services;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Uniformances;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.CommunicationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.CoolingMediumSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.GlycolRegenerationSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.HealthSafetyAndEnvironment;
using JXNippon.CentralizedDatabaseSystem.Shared.LivingQuartersUtilitiesAndOthers;
using JXNippon.CentralizedDatabaseSystem.Shared.Logistic;
using JXNippon.CentralizedDatabaseSystem.Shared.LWPActivity;
using JXNippon.CentralizedDatabaseSystem.Shared.MajorEquipment;
using JXNippon.CentralizedDatabaseSystem.Shared.MaximoWorkOrder;
using JXNippon.CentralizedDatabaseSystem.Shared.OIMSummary;
using JXNippon.CentralizedDatabaseSystem.Shared.PowerGenerationAndDistributionManagement;
using JXNippon.CentralizedDatabaseSystem.Shared.ProducedWaterTreatmentSystemManagement;
using JXNippon.CentralizedDatabaseSystem.Shared.RollsRoyceGasEngineAndKawasakiCompressionSystem;
using JXNippon.CentralizedDatabaseSystem.Shared.VendorActivities;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHead;
using JXNippon.CentralizedDatabaseSystem.Shared.WellHeadAndSeparationSystem;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportView
    {
        private static readonly HashSet<Type> RequiredTypes = new HashSet<Type>()
        {
            typeof(string),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?),
            typeof(decimal),
            typeof(decimal?),
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?),
        };
        private bool isEditing;
        private bool isLoading = true;
        private bool isUserHavePermission = true;

        [Parameter] public CombinedDailyReport Data { get; set; }

        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }

        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        [Inject] private IUserService UserService { get; set; }

        [Inject] private IReportService ReportService { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; }

        private List<UniformanceResult> uniformanceNotInToleranceResults { get; set; }
        private List<UniformanceResult> uniformanceErrorResults { get; set; }
        private CommonFilter CommonFilter { get; set; }
        private FPSOHelangSummaryDataGrid fpsoHelangSummaryDataGrid;
        private HIPAndLWPSummaryDataGrid hipAndLWPSummaryDataGrid;
        private LongTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid;
        private OperatingChangeDataGrid operatingChangeDataGrid;
        private MajorEquipmentStatusDataGrid majorEquipmentStatusDataGrid;
        private WellStreamCoolerDataGrid wellStreamCoolerDataGrid;
        private HIPWellHeadParameterDataGrid hipWellHeadParameterDataGrid;
        private LWPWellHeadParameterDataGrid lwpWellHeadParameterDataGrid;
        private RollsRoyceRB211EngineDataGrid rollsRoyceRB211EngineDataGrid;
        private KawasakiExportCompressorDataGrid kawasakiExportCompressorDataGrid;
        private KawasakiExportCompressorDataGridPart2 kawasakiExportCompressorDataGridPart2;
        private GlycolPumpDataGrid glycolPumpDataGrid;
        private GlycolTrainDataGrid glycolTrainDataGrid;
        private CoolingMediumSystemDataGrid coolingMediumSystemDataGrid;
        private PowerGenerationAndDistributionManagementDataGrid powerGenerationAndDistributionManagementDataGrid;
        private UtilitiesDataGrid utilitiesDataGrid;
        private ProducedWaterTreatmentSystemManagementDataGrid producedWaterTreatmentSystemManagementDataGrid;
        private LWPActivityDataGrid lWPActivityDataGrid;
        private CommunicationSystemDataGrid communicationSystemDataGrid;
        private VendorActivitiesDataGrid vendorActivitiesDataGrid;
        private LogisticDataGrid logisticDataGrid;
        private MaximoWorkOrderDataGrid maximoWorkOrderDataGrid;
        private LifeBoatsDataGrid lifeBoatsDataGrid;

        private CombinedDailyReportTag combinedDailyReportTagRef
        {
            set
            {
                combinedDailyReportTags.Add(value);
            }
        }
        private readonly ICollection<CombinedDailyReportTag> combinedDailyReportTags = new List<CombinedDailyReportTag>();
        private void SetIsEditing(bool value)
        {
            isEditing = value;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager)
            {
                Date = Data.Date.UtcDateTime
            };

            isLoading = false;
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.CombinedDailyReport), HasReadPermissoin = true, HasWritePermission = true });
            await base.OnInitializedAsync();
        }

        private IGenericService<CombinedDailyReport> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<CombinedDailyReport, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private void CalculateUniformanceValidationResult()
        {
            uniformanceErrorResults = new List<UniformanceResult>();
            uniformanceNotInToleranceResults = new List<UniformanceResult>();
            //DailyHIPProduction
            CalculateUniformanceResult(this.Data.DailyHIPProduction);

            //DailyCINalco
            CalculateUniformanceResult(this.Data.DailyCiNalco);

            //DailyWellHeadSeparationSystem
            CalculateUniformanceResult(this.Data.DailyWellHeadAndSeparationSystem);

            //DailyWellStreamCooler
            CalculateUniformanceResult(this.Data.DailyWellStreamCoolers.ToArray());

            //DailyHIPWellHeadParameter
            CalculateUniformanceResult(this.Data.DailyHIPWellHeadParameters.ToArray());

            //DailyLWPWellHeadParameter
            CalculateUniformanceResult(this.Data.DailyLWPWellHeadParameters.ToArray());

            //DailyKawasaki
            CalculateUniformanceResult(this.Data.DailyKawasakiExportCompressors.ToArray());

            //DailyRollsRoyce
            CalculateUniformanceResult(this.Data.DailyRollsRoyceRB211Engines.ToArray());

            //DailyGlycolTrain
            CalculateUniformanceResult(this.Data.DailyGlycolTrains.ToArray());

            //DailyGasexportLine
            CalculateUniformanceResult(this.Data.DailyGasCondensateExportSamplerAndExportLine);
        }

        private void CalculateUniformanceResult(params IUniformanceValidation[] uniformanceValidations)
        {
            foreach (var uniformanceValidation in uniformanceValidations)
            {
                var errorList = uniformanceValidation.UniformanceResults
                .Where(x => x.ValidationResult == UniformanceResultStatus.UniformanceError)
                .ToList();

                if (errorList.Count > 0)
                {
                    this.uniformanceErrorResults.AddRange(errorList);
                }

                var notInToleranceList = uniformanceValidation.UniformanceResults
                    .Where(x => x.ValidationResult == UniformanceResultStatus.NotInTolerance)
                    .ToList();

                if (notInToleranceList.Count > 0)
                {
                    this.uniformanceNotInToleranceResults.AddRange(notInToleranceList);
                }
            }
		}

		private async Task<bool> ShowDialogAsync(int errorCount, int notInToleranceCount)
		{
			dynamic? response;
            string confirmationText = $"This report have {errorCount} uniformance error and {notInToleranceCount} not within tolerance. Are you sure to approve?";
            response = await DialogService.OpenAsync<GenericConfirmationDialog>("Approval",
						   new Dictionary<string, object>() { ["ConfirmationText"] = confirmationText},
						   new Radzen.DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

            return response == true;
		}

		private async Task ApproveAsync()
        {
            bool isProceed = true;
            try
            {
                if (this.Data.Status == CombinedDailyReportStatus.Approved)
                {
                    AffraNotificationService.NotifyInfo("The report has already been approved.");
                }
                else if (CanApprove())
                {
                    CalculateUniformanceValidationResult();

                    if (uniformanceNotInToleranceResults.Count > 0
                        || uniformanceErrorResults.Count > 0)
                    {
                        isProceed = await ShowDialogAsync(uniformanceErrorResults.Count, uniformanceNotInToleranceResults.Count);
                    }

                    if (isProceed)
                    {
						using var scope = ServiceProvider.CreateScope();
						var service = GetGenericService(scope);
						var referenceId = await this.ReportService.GenerateCombinedDailyReportAsync(Data);
						Data.Status = CombinedDailyReportStatus.Approved;
						Data.Revision++;
						Data.User = this.GlobalDataSource.User.Email;
						Data.LastApproval = new()
						{
							ApprovedDateTime = DateTime.UtcNow,
							ReportReferenceId = referenceId,
							Revision = Data.Revision,
							ApprovedBy = this.GlobalDataSource.User.Name,
							FileName = $"COMBINED DAILY REPORT {Data.DateUI:yyyyMMdd} Rev{Data.Revision}.xlsx",
						};
						Data.Approvals.Add(Data.LastApproval);
						await service.UpdateAsync(Data, Data.Id);
						AffraNotificationService.NotifySuccess("Report approved.");
						DialogService.Close();
					}
                }
                else
                {
                    AffraNotificationService.NotifyWarning("Please complete all the required fields.");
                }

            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }

        private async Task RejectAsync()
        {
            try
            {
                using var scope = ServiceProvider.CreateScope();
                var service = GetGenericService(scope);
                Data.Status = CombinedDailyReportStatus.Rejected;
                Data.User = this.GlobalDataSource.User.Email;
                await service.UpdateAsync(Data, Data.Id);
                AffraNotificationService.NotifySuccess("Report rejected.");
                DialogService.Close();
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }

        private async Task LoadFPSOHelangSummaryDataGridAsync(LoadDataArgs args)
        {
            fpsoHelangSummaryDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadHIPAndLWPSummaryDataGridAsync(LoadDataArgs args)
        {
            hipAndLWPSummaryDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLongTermOverridesAndInhibitsOnAlarmAndOrTripDataGridAsync(LoadDataArgs args)
        {
            longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadOperatingChangeDataGridAsync(LoadDataArgs args)
        {
            operatingChangeDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadMajorEquipmentDataGridAsync(LoadDataArgs args)
        {
            majorEquipmentStatusDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadWellStreamCoolerDataGridAsync(LoadDataArgs args)
        {
            wellStreamCoolerDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadHIPWellHeadParameterDataGridAsync(LoadDataArgs args)
        {
            hipWellHeadParameterDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLWPWellHeadParameterDataGridAsync(LoadDataArgs args)
        {
            lwpWellHeadParameterDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadRollsRoyceRB211EngineDataGridAsync(LoadDataArgs args)
        {
            rollsRoyceRB211EngineDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadKawasakiExportCompressorDataGridAsync(LoadDataArgs args)
        {
            kawasakiExportCompressorDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadKawasakiExportCompressorDataGridPart2Async(LoadDataArgs args)
        {
            kawasakiExportCompressorDataGridPart2.CommonFilter = CommonFilter;
        }
        private async Task LoadGlycolPumpDataGridAsync(LoadDataArgs args)
        {
            glycolPumpDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadGlycolTrainDataGridAsync(LoadDataArgs args)
        {
            glycolTrainDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadCoolingMediumSystemDataGridAsync(LoadDataArgs args)
        {
            coolingMediumSystemDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadPowerGenerationAndDistributionManagementDataGridAsync(LoadDataArgs args)
        {
            powerGenerationAndDistributionManagementDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadUtilitiesDataGridAsync(LoadDataArgs args)
        {
            utilitiesDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadProducedWaterTreatmentSystemManagementDataGridAsync(LoadDataArgs args)
        {
            producedWaterTreatmentSystemManagementDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLWPActivityDataGridAsync(LoadDataArgs args)
        {
            lWPActivityDataGrid.CommonFilter = CommonFilter;
        }

        private async Task LoadCommunicationSystemDataGridAsync(LoadDataArgs args)
        {
            communicationSystemDataGrid.DailyDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadVendorActivitiesDataGridAsync(LoadDataArgs args)
        {
            vendorActivitiesDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLogisticDataGridAsync(LoadDataArgs args)
        {
            logisticDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadMaximoWorkOrderDataGridAsync(LoadDataArgs args)
        {
            maximoWorkOrderDataGrid.CommonFilter = CommonFilter;
        }
        private async Task LoadLifeBoatsDataGridAsync(LoadDataArgs args)
        {
            lifeBoatsDataGrid.CommonFilter = CommonFilter;
        }

        private int GetCollectionTotalUnfillProperty<T>(Collection<T> collection, string extraExemption = "")
        {
            if (collection is null || collection.Count == 0)
            {
                return -1;
            }
            return collection.Sum(x => GetTotalUnfillProperty(x));
        }

        private string[] GetTotalUnfillPropertyName(object property, string extraExemption = "")
        {
            if (property is null)
            {
                return null;
            }

            var query = property.GetType().GetProperties()
                    .Where(x => x.GetValue(property) is null)
                    .Where(x => !x.Name.Contains("Remark"))
                    .Where(x => !x.Name.Contains("Extras"))
                    .Where(x => !x.Name.Equals("Id"))
                    .Where(x => !x.Name.Equals("xmin"))
                    .Where(x => !x.Name.Equals("CombinedDailyReport"))
                    .Where(x => !x.Name.Equals("SystemValidateDateTime"))
                    .Where(x => !x.Name.Equals("UserValidationDateTime"))
                    .Where(x => !x.Name.Equals("ValidationResults"))
                    .Where(x => RequiredTypes.Contains(x.PropertyType));

            if (!string.IsNullOrEmpty(extraExemption))
            {
                query = query.Where(x => x.Name == extraExemption);
            }

            var result = query
                .Select(x => x.Name)
                .ToArray();

            Console.WriteLine(string.Join(',', result));

            return result;
        }

        private int GetTotalUnfillProperty(object property, string extraExemption = "")
        {
            var result = GetTotalUnfillPropertyName(property, extraExemption);
            return result is null
                ? -1
                : result.Length;
        }

        private bool CanApprove()
        {
            return combinedDailyReportTags
                .All(x => x.HasNoViolation);
        }

        private async Task DownloadAsync()
        {
            isLoading = true;
            try
            {
                if (Data.LastApproval is null)
                {
                    throw new InvalidOperationException("Report never approved before.");
                }
                var streamResult = await ReportService.DownloadReportAsync(Data.LastApproval.ReportReferenceId);
                if (streamResult != null)
                {
                    using var streamRef = new DotNetStreamReference(streamResult);
                    await JSRuntime.InvokeVoidAsync("downloadFileFromStream", Data.LastApproval.FileName, streamRef);
                }
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
            isLoading = false;
        }
    }
}
