using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Domain.Notifications;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.CloseOuts;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Extensions;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Identifications;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.SCEElements;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using OpenAPI.UserService;
using Radzen;
using System;
using System.Collections.Generic;
using Message = NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages.Message;
using User = UserODataService.Affra.Service.User.Domain.Users.User;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class MOCFormTemplate
    {
        [Parameter] public ManagementOfChangeRecord Item { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IUserServiceClient UserServiceClient { get; set; }
        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }
        [Inject] private ConfirmService ConfirmService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        private ICollection<string> sceCodes;
        private ICollection<string> userDesignation;
        private ICollection<string> lineManagerUsername;
        private ICollection<string> approveAuthorityUsername;
        private ICollection<string> closeOutAuthorityUsername;
        private int currentStep;
        private bool disableAllInput;
        private bool disableAddExtensionButton;
        private bool disableDurationOfChange;
        private readonly IDictionary<string, RiskLevels> riskMatrixDictionary = new Dictionary<string, RiskLevels>();
        protected override async Task OnInitializedAsync()
        {
            disableAllInput = false;
            disableAddExtensionButton = false;

            switch (Item.ManagementOfChangeCurrentStep)
            {
                case ManagementOfChangeCurrentStep.InitialCreation:
                    currentStep = 0;
                    break;
                case ManagementOfChangeCurrentStep.RiskEvaluation:
                    currentStep = 1;
                    break;
                case ManagementOfChangeCurrentStep.EndorsementPendingForApproval:
                    disableAllInput = true;
                    currentStep = 2;
                    break;
                case ManagementOfChangeCurrentStep.EndorsementSubmitForApproval:
                    currentStep = 2;
                    await LoadLineManagersAsync();
                    break;
                case ManagementOfChangeCurrentStep.AuthorisationAndApprovalPendingForApproval:
                    disableAllInput = true;
                    currentStep = 3;
                    break;
                case ManagementOfChangeCurrentStep.AuthorisationAndApprovalSubmitForApproval:
                    currentStep = 3;
                    await LoadApprovingAuthoritiesAsync();
                    break;
                case ManagementOfChangeCurrentStep.ExtensionPendingForApproval:
                    disableAddExtensionButton = true;
                    disableAllInput = true;
                    currentStep = 4;
                    break;
                case ManagementOfChangeCurrentStep.CloseoutPendingForApproval:
                    disableAllInput = true;
                    currentStep = 4;
                    break;
                case ManagementOfChangeCurrentStep.ExtensionSubmitForApproval:
                case ManagementOfChangeCurrentStep.CloseOutSubmitForApproval:
                    currentStep = 4;
                    await LoadCloseOutAuthoritiesAsync();
                    break;
                case ManagementOfChangeCurrentStep.Completed:
                    disableAddExtensionButton = true;
                    disableAllInput = true;
                    currentStep = 0;
                    break;
            }

            if(Item.Identification.DetailOfChange == DetailOfChange.FacilitiesImprovementPlan && Item.CloseOut.Signature != string.Empty)
            {
                disableDurationOfChange = true;
            }
            else if(Item.Identification.DetailOfChange != DetailOfChange.FacilitiesImprovementPlan && Item.AuthorisationAndApproval.Signature != string.Empty)
            {
                disableDurationOfChange = true;
            }
            else
            {
                disableDurationOfChange = false;
            }

            InitRiskMatrixDictionary();
            await LoadSCECodeAsync();
            await LoadUserDataAsync();
        }
        private async Task LoadUserDataAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<User>? userService = GetGenericService(serviceScope);
            var query = userService.Get();

            Microsoft.OData.Client.QueryOperationResponse<User>? usersResponse = await query
                .OrderByDescending(x => x.Role)
                .ToQueryOperationResponseAsync<User>();

            userDesignation = usersResponse
                .Select(x => x.Role)
                .ToList();

            StateHasChanged();
        }
        private async Task LoadSCECodeAsync()
        {
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<SCEElementRecord>? sceService = GetGenericSCEService(serviceScope);
                var query = sceService.Get();

                Microsoft.OData.Client.QueryOperationResponse<SCEElementRecord>? sceResponse = await query
                    .OrderBy(x => x.SCECode)
                    .ToQueryOperationResponseAsync<SCEElementRecord>();

                sceCodes = sceResponse
                    .Select(x => x.SCECode)
                    .ToList();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }          
        }
        private async Task OnNextButtonClick()
        {
            if (Item.Identification.DetailOfChange == DetailOfChange.FacilitiesImprovementPlan)
            {
                currentStep = 4;
            }
            else
            {
                if (Item.ManagementOfChangeCurrentStep == ManagementOfChangeCurrentStep.RiskEvaluation)
                {
                    await LoadLineManagersAsync();
                    Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.EndorsementSubmitForApproval;
                    currentStep++;
                }
                else
                {
                    currentStep++;
                }
            }
        }
        private void OnPreviousButtonClick()
        {
            if (Item.Identification.DetailOfChange == DetailOfChange.FacilitiesImprovementPlan)
            {
                currentStep = 0;
            }
            else
            {
                currentStep--;
            }
        }
        private async void OnCreateButtonClick()
        {
            try
            {
                if (Item.TitleOfChange == string.Empty)
                {
                    AffraNotificationService.NotifyWarning("Title of change is required.");
                    return;
                }
                if (Item.Identification.DescriptionOfChange == string.Empty)
                {
                    AffraNotificationService.NotifyWarning("Description of change is required.");
                    return;
                }
                if (Item.Identification.DurationOfChange <= 0)
                {
                    AffraNotificationService.NotifyWarning("Duration of change cannot be less than or equal 0.");
                    return;
                }
                if (Item.Identification.SCETagNumber == string.Empty)
                {
                    AffraNotificationService.NotifyWarning("SCE tag number is required.");
                    return;
                }

                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.New;

                if (Item.Identification.DetailOfChange == DetailOfChange.FacilitiesImprovementPlan)
                {
                    Item.CloseOut.CloseOutState = CloseOutState.MadePermanentState;
                    Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.CloseOutSubmitForApproval;
                    currentStep = 4;
                    await LoadCloseOutAuthoritiesAsync();
                }
                else
                {
                    Item.CloseOut.CloseOutState = CloseOutState.RevertOriginalState;
                    Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.RiskEvaluation;
                    currentStep = 1;
                }

                Person person = await UserServiceClient.Person_GetPersonAsync(GlobalDataSource.User.Email);

                string categoriesOfChange = "";
                if (Item.Identification.DetailOfChange == DetailOfChange.FacilitiesImprovementPlan)
                {
                    categoriesOfChange = "FIP";
                }
                else if (Item.Identification.DetailOfChange == DetailOfChange.OperatingChangeAndNonRoutineOperations)
                {
                    categoriesOfChange = "OCR";
                }
                else
                {
                    categoriesOfChange = "PCSC";
                }

                Item.RecordNumber = $"MOC-{person.Department}-{Item.ManagementOfChangeField}-{categoriesOfChange}-{DateTime.Now:yyyy}-{DateTime.Now:yyyyMMddHHmmss}";

                using var serviceScope = ServiceProvider.CreateScope();
                var service = GetGenericMOCService(serviceScope);

                await service.InsertAsync(Item);

                AffraNotificationService.NotifyItemCreated();
            }
            catch(Exception ex) 
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private async Task LoadLineManagersAsync()
        {
            lineManagerUsername = await GetLineManagersAndDelegationsAsync();

            StateHasChanged();
        }
        private async Task LoadApprovingAuthoritiesAsync()
        {
            approveAuthorityUsername = await GetLineManagersAndDelegationsAsync();

            StateHasChanged();
        }
        private async Task LoadCloseOutAuthoritiesAsync()
        {
            closeOutAuthorityUsername = await GetLineManagersAndDelegationsAsync();

            StateHasChanged();
        }
        private async Task<string> LoadExtensionAuthoritiesAsync()
        {
            try
            {
                Person person = await UserServiceClient.Person_GetPersonAsync(GlobalDataSource.User.Email);

                string extensionManagers = "";

                foreach (string manager in await UserServiceClient.Manager_GetManagersAsync(person.Department))
                {
                    extensionManagers += $"{manager},";

                    foreach (string delegation in await UserServiceClient.Delegation_GetDelegationsAsync(manager, person.Department))
                    {
                        extensionManagers += $"{delegation},";
                    }
                }
                return extensionManagers;
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
            return string.Empty;
        }

        private async Task<ICollection<string>> GetLineManagersAndDelegationsAsync()
        {
            try 
            {
                Person person = await UserServiceClient.Person_GetPersonAsync(GlobalDataSource.User.Email);

                var results = new List<string>();
                if (string.IsNullOrEmpty(person.Department))
                {
                    throw new ArgumentNullException("No department found!");
                }
                foreach (string manager in await UserServiceClient.Manager_GetManagersAsync(person.Department))
                {
                    results.Add(manager);

                    foreach (string delegation in await UserServiceClient.Delegation_GetDelegationsAsync(manager, person.Department))
                    {
                        results.Add(delegation);
                    }
                }

                return results.Distinct().ToList();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
            return Array.Empty<string>();            
        }

        private void OnCloseDialogClicked()
        {
            DialogService.Close(false);
        }
        private async Task OnEndorsementSubmitButtonClickAsync()
        {
            if (Item.Endorsement.Name == string.Empty)
            {
                AffraNotificationService.NotifyWarning("Endorsement approval name is required.");
                return;
            }

            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.PendingForApproval;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.EndorsementPendingForApproval;

            await SubmitAsync(Item);

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
            var query = notificationService.InsertAsync(new Message
            {
                Subject = Item.RecordNumber,
                Content = "You have a new management of change to approve!",
                Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.Endorsement.Name
                }
            });
        }
        private async Task OnAuthorisationAndApprovalSubmitButtonClickAsync()
        {
            if (Item.AuthorisationAndApproval.Name == string.Empty)
            {
                AffraNotificationService.NotifyWarning("Authorisation approval name is required.");
                return;
            }

            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.PendingForApproval;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.AuthorisationAndApprovalPendingForApproval;

            await SubmitAsync(Item);

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
            var query = notificationService.InsertAsync(new Message
            {
                Subject = Item.RecordNumber,
                Content = "You have a new management of change to approve!",
                Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.AuthorisationAndApproval.Name
                }
            });
        }
        private async Task OnExtensionSubmitButtonClickAsync()
        {
            if (Item.Extensions[Item.Extensions.Count - 1].ApproverName == string.Empty)
            {
                AffraNotificationService.NotifyWarning("Extension approval name is required.");
                return;
            }
            if (Item.Extensions[Item.Extensions.Count - 1].DurationExtended <= 0)
            {
                AffraNotificationService.NotifyWarning("Duration extended cannot less than or equal to 0.");
                return;
            }

            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Extension;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.ExtensionPendingForApproval;

            await SubmitAsync(Item);

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
            var query = notificationService.InsertAsync(new Message
            {
                Subject = Item.RecordNumber,
                Content = "You have a new management of change to approve!",
                Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.Extensions[Item.Extensions.Count - 1].ApproverName
                }
            });
        }
        private async Task OnCloseOutSubmitButtonClickAsync()
        {
            if (Item.CloseOut.Name == string.Empty)
            {
                AffraNotificationService.NotifyWarning("Manager is required.");
                return;
            }

            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.PendingForApproval;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.CloseoutPendingForApproval;

            await SubmitAsync(Item);

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
            var query = notificationService.InsertAsync(new Message
            {
                Subject = Item.RecordNumber,
                Content = "You have a new management of change to approve!",
                Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CloseOut.Name
                }
            });
        }
        private async Task OnAddExtensionButtonClickAsync()
        {
            if (Item.Extensions.Count == 5)
            {
                AffraNotificationService.NotifyWarning("Maximum extension reached.");
                return;
            }

            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Extension;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.ExtensionSubmitForApproval;

            Item.Extensions.Add(new Extension
            {
                ExtensionNo = Item.Extensions.Count + 1,
                DurationExtended = 180,
                ReviewDate = DateTimeOffset.UtcNow,
                ApproverNameColection = await LoadExtensionAuthoritiesAsync(),
                ApproverName = string.Empty,
                ApproverDesignation = string.Empty,
                ApprovalSignature = string.Empty
            });
            disableAllInput = false;
            disableAddExtensionButton = true;
        }
        private void OnStepChange(int current)
        {
            currentStep = current;
        }
        protected Task SubmitAsync(ManagementOfChangeRecord args, bool closeDialog = true)
        {
            if (closeDialog)
            {
                DialogService.Close(true);
            }

            return Task.CompletedTask;
        }
        private IGenericService<User> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<User, IUserUnitOfWork>>();
        }
        private async Task ShowCloseOutConfirmationDialogAsync()
        {
            var content = $"Are you sure want to approve record no #{Item.RecordNumber} ?";
            var title = "Management of Change - Close-Out";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.Completed;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.PendingForCloseOut;
                Item.CloseOut.Signature = Item.CloseOut.Name;
                Item.CloseOut.DateUI = DateTime.Now;

                if(Item.Identification.DetailOfChange == DetailOfChange.FacilitiesImprovementPlan)
                {
                    Item.Identification.ExpiryDateUI = DateTime.Now.AddDays(Item.Identification.DurationOfChange);
                }

                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.RecordNumber,
                    Content = "Your management of change has been approved!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CreatedBy
                }
                });
            }
        }
        private async Task ShowCloseOutRejectDialogAsync()
        {
            var content = $"Are you sure want to reject record no #{Item.RecordNumber} ?";
            var title = "Management of Change - Close-Out";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.CloseOutSubmitForApproval;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Rejected;
                Item.CloseOut.Name = string.Empty;
                Item.CloseOut.Signature = string.Empty;
                Item.CloseOut.DateUI = DateTime.Now;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.RecordNumber,
                    Content = "Your management of change has been rejected!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CreatedBy
                }
                });
            }
        }
        private async Task ShowEndorsementConfirmationDialogAsync()
        {
            var content = $"Are you sure want to approve record no #{Item.RecordNumber} ?";
            var title = "Management of Change - Endorsement";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.AuthorisationAndApprovalSubmitForApproval;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Approved;
                Item.Endorsement.Signature = Item.Endorsement.Name;
                Item.Endorsement.DateUI = DateTime.Now;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.RecordNumber,
                    Content = "Your management of change has been approved!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CreatedBy
                }
                });
            }
        }
        private async Task ShowEndorsementRejectDialogAsync()
        {
            var content = $"Are you sure want to reject record no #{Item.RecordNumber} ?";
            var title = "Management of Change - Endorsement";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.EndorsementSubmitForApproval;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Rejected;
                Item.Endorsement.DateUI = DateTime.Now;
                Item.Endorsement.Name = string.Empty;
                Item.Endorsement.Signature = string.Empty;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.RecordNumber,
                    Content = "Your management of change has been rejected!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CreatedBy
                }
                });
            }
        }
        private async Task ShowAuthorisationAndApprovalConfirmationDialogAsync()
        {
            var content = $"Are you sure want to approve record no #{Item.RecordNumber} ?";
            var title = "Management of Change - Authorisation & Approval";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.CloseOutSubmitForApproval;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Approved;
                Item.AuthorisationAndApproval.Signature = Item.AuthorisationAndApproval.Name;
                Item.AuthorisationAndApproval.DateUI = DateTime.Now;

                if (Item.Identification.DetailOfChange != DetailOfChange.FacilitiesImprovementPlan)
                {
                    Item.Identification.ExpiryDateUI = DateTime.Now.AddDays(Item.Identification.DurationOfChange);
                }

                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.RecordNumber,
                    Content = "Your management of change has been approved!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CreatedBy
                }
                });
            }
        }
        private async Task ShowAuthorisationAndApprovalRejectDialogAsync()
        {
            var content = $"Are you sure want to reject record no #{Item.RecordNumber} ?";
            var title = "Management of Change - Authorisation & Approval";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.AuthorisationAndApprovalSubmitForApproval;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Rejected;
                Item.AuthorisationAndApproval.DateUI = DateTime.Now;
                Item.AuthorisationAndApproval.Name = string.Empty;
                Item.AuthorisationAndApproval.Signature = string.Empty;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.RecordNumber,
                    Content = "Your management of change has been rejected!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CreatedBy
                }
                });
            }
        }
        private async Task ShowExtensionConfirmationDialogAsync(Extension item)
        {
            var content = $"Are you sure want to approve record no #{Item.RecordNumber} ?";
            var title = $"Management of Change - Extension #{item.ExtensionNo}";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.Extensions[item.ExtensionNo - 1].ApprovalSignature = Item.Extensions[item.ExtensionNo - 1].ApproverName;
                Item.Extensions[item.ExtensionNo - 1].ReviewDateUI = DateTime.Now;

                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.CloseOutSubmitForApproval;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Approved;

                Item.Identification.ExpiryDateUI = DateTime.Now.AddDays(Item.Extensions[item.ExtensionNo - 1].DurationExtended);

                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.RecordNumber,
                    Content = "Your management of change has been approved!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CreatedBy
                }
                });
            }
        }
        private async Task ShowExtensionRejectDialogAsync(Extension item)
        {
            var content = $"Are you sure want to reject record no #{Item.RecordNumber} ?";
            var title = $"Management of Change - Extension #{item.ExtensionNo}";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.ExtensionSubmitForApproval;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Extension;
                Item.CloseOut.DateUI = DateTime.Now;
                Item.Extensions[item.ExtensionNo - 1].ApproverName = string.Empty;
                Item.Extensions[item.ExtensionNo - 1].ApprovalSignature = string.Empty;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.RecordNumber,
                    Content = "Your management of change has been rejected!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.CreatedBy
                }
                });
            }
        }
        private void OnInheritRadioChanged()
        {
            string inheritLikelihoodLevel = Item.RiskEvaluation.InheritRiskLikelihood.ToString();
            string inheritConsequenceLevel = Item.RiskEvaluation.InheritRiskConsequence.ToString();
            string combinedInheritRiskLevel = $"{inheritLikelihoodLevel.Substring(inheritLikelihoodLevel.Length - 1)}{inheritConsequenceLevel.Substring(inheritConsequenceLevel.Length - 1)}";

            Item.RiskEvaluation.InheritRiskRiskLevel = riskMatrixDictionary[combinedInheritRiskLevel];
        }
        private void OnResidualRadioChanged()
        {
            string residualLikelihoodLevel = Item.RiskEvaluation.ResidualRiskLikelihood.ToString();
            string residualConsequenceLevel = Item.RiskEvaluation.ResidualRiskConsequence.ToString();
            string combinedResidualRiskLevel = $"{residualLikelihoodLevel.Substring(residualLikelihoodLevel.Length - 1)}{residualConsequenceLevel.Substring(residualConsequenceLevel.Length - 1)}";

            Item.RiskEvaluation.ResidualRiskRiskLevel = riskMatrixDictionary[combinedResidualRiskLevel];
        }
        private void InitRiskMatrixDictionary()
        {
            riskMatrixDictionary.Clear();

            riskMatrixDictionary.Add("A1", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("A2", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("A3", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("A4", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("B1", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("B2", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("B3", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("C1", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("C2", RiskLevels.LowRisk);
            riskMatrixDictionary.Add("D1", RiskLevels.LowRisk);

            riskMatrixDictionary.Add("E1", RiskLevels.MediumRisk);
            riskMatrixDictionary.Add("D2", RiskLevels.MediumRisk);
            riskMatrixDictionary.Add("C3", RiskLevels.MediumRisk);
            riskMatrixDictionary.Add("B4", RiskLevels.MediumRisk);
            riskMatrixDictionary.Add("A5", RiskLevels.MediumRisk);

            riskMatrixDictionary.Add("E2", RiskLevels.HighRisk);
            riskMatrixDictionary.Add("E3", RiskLevels.HighRisk);
            riskMatrixDictionary.Add("D3", RiskLevels.HighRisk);
            riskMatrixDictionary.Add("D4", RiskLevels.HighRisk);
            riskMatrixDictionary.Add("C4", RiskLevels.HighRisk);
            riskMatrixDictionary.Add("C5", RiskLevels.HighRisk);
            riskMatrixDictionary.Add("B5", RiskLevels.HighRisk);

            riskMatrixDictionary.Add("E4", RiskLevels.VeryHighRisk);
            riskMatrixDictionary.Add("E5", RiskLevels.VeryHighRisk);
            riskMatrixDictionary.Add("D5", RiskLevels.VeryHighRisk);
        }
        private IGenericService<ManagementOfChangeRecord> GetGenericMOCService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<ManagementOfChangeRecord, IManagementOfChangeUnitOfWork>>();
        }
        private IGenericService<SCEElementRecord> GetGenericSCEService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<SCEElementRecord, IManagementOfChangeUnitOfWork>>();
        }
        private IGenericService<Message> GetGenericNotificationService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<Message, INotificationUnitOfWork>>();
        }
    }
}
