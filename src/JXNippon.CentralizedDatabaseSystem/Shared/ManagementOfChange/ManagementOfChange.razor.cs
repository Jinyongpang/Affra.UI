using Affra.Core.Domain.Services;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.CloseOuts;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Identifications;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class ManagementOfChange
    {
        private const string All = "All";
        private ManagementOfChangeDataList managementOfChangeDataList;
        private Menu menu;
        private string search;
        private string status;
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        private Task ReloadAsync(string status = null)
        {
            managementOfChangeDataList.ManagementOfChangeFilter.Status = GetStatusFilter(status ?? this.status);
            managementOfChangeDataList.ManagementOfChangeFilter.Search = search;
            return managementOfChangeDataList.ReloadAsync();
        }

        private string GetStatusFilter(string status = null)
        {
            status ??= menu.SelectedKeys.FirstOrDefault();
            if (status != All)
            {
                return status;
            }

            return null;
        }

        private Task OnMenuItemSelectAsync(MenuItem menuItem)
        {
            status = menuItem.Key;
            return this.ReloadAsync(menuItem.Key);
        }
        private async Task ShowDialogAsync(ManagementOfChangeRecord data, string title = "")
        {
            data = new()
            {
                TitleOfChange = string.Empty,
                RecordNumber = "",
                ManagementOfChangeField = ManagementOfChangeField.HIP,
                CreateDateTimeUI = DateTimeOffset.Now.Date,
                CreatedBy = string.Empty,
                ManagementOfChangeStatus = ManagementOfChangeStatus.New,
                ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.InitialCreation,
                Identification = new()
                {
                    DurationOfChange = 0,
                    ExpiryDateUI = DateTimeOffset.Now.Date,
                    DescriptionOfChange = string.Empty,
                    ReasonForChange = ReasonForChange.Deviation,
                    OtherReasonForChange = string.Empty,
                    DetailOfChange = DetailOfChange.FacilitiesImprovementPlan,
                    CategoriesOfChange = CategoriesOfChange.Permanent,
                    PriorityOfChange = PriorityOfChange.Normal,
                    SCEAffected = false,
                    SCETagNumber = string.Empty,
                },
                RiskEvaluation = new()
                {
                    InheritRiskLikelihood = LikelihoodLevels.LikelihoodLevelA,
                    InheritRiskConsequence = ConsequenceLevels.ConsequenceLevel1,
                    InheritRiskRiskLevel = RiskLevels.LowRisk,
                    ResidualRiskLikelihood = LikelihoodLevels.LikelihoodLevelA,
                    ResidualRiskConsequence = ConsequenceLevels.ConsequenceLevel1,
                    ResidualRiskRiskLevel = RiskLevels.LowRisk,
                    RecommendationAndCountermeasure = string.Empty
                },
                Endorsement = new()
                {
                    LineManagerComment = string.Empty,
                    Name = string.Empty,
                    Designation = string.Empty,
                    Signature = string.Empty,
                    DateUI = DateTimeOffset.Now.Date
                },
                AuthorisationAndApproval = new()
                {
                    ApprovingAuthorityComment = string.Empty,
                    Name = string.Empty,
                    Designation = string.Empty,
                    Signature = string.Empty,
                    DateUI = DateTimeOffset.Now.Date
                },
                CommunicationAndImplementation = new()
                {
                    Comments = string.Empty,
                    PreparedByName = string.Empty,
                    PreparedBySignature = string.Empty,
                    PreparedByDateUI = DateTimeOffset.Now.Date,
                    AgreedByName = string.Empty,
                    AgreedBySignature = string.Empty,
                    AgreedByDateUI = DateTimeOffset.Now.Date
                },
                CloseOut = new()
                {
                    CloseOutState = CloseOutState.MadePermanentState,
                    Name = string.Empty,
                    Designation = string.Empty,
                    Signature = string.Empty,
                    DateUI = DateTimeOffset.Now.Date
                }
            };

            dynamic? response;
            response = await DialogService.OpenAsync<MOCFormTemplate>("",
                new Dictionary<string, object>() { { "item", data } },
                Constant.MOHDialogOptions);

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericMOCService(serviceScope);

                    if (data.Id > 0)
                    {
                        await service.UpdateAsync(data, data.Id);
                        AffraNotificationService.NotifyItemUpdated();
                    }
                    else
                    {
                        await service.InsertAsync(data);
                        AffraNotificationService.NotifyItemCreated();
                    }

                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
            }

            await ReloadAsync();
        }
        private IGenericService<ManagementOfChangeRecord> GetGenericMOCService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<ManagementOfChangeRecord, IManagementOfChangeUnitOfWork>>();
        }
    }
}