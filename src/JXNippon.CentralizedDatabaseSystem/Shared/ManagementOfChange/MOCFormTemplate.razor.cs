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

        private List<string> sceCodes;
        private List<string> userDesignation;
        private List<string> lineManagerUsername;
        private List<string> approveAuthorityUsername;
        private List<string> closeOutAuthorityUsername;
        private int currentStep;
        private bool disableAllInput;
        private bool disableAddExtensionButton;
        private Dictionary<string, RiskLevels> riskMatrixDictionary = new Dictionary<string, RiskLevels>();
        protected async override Task OnInitializedAsync()
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
                    break;
                case ManagementOfChangeCurrentStep.AuthorisationAndApprovalPendingForApproval:
                    disableAllInput = true;
                    currentStep = 3;
                    break;
                case ManagementOfChangeCurrentStep.AuthorisationAndApprovalSubmitForApproval:
                    currentStep = 3;
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
                    break;
                case ManagementOfChangeCurrentStep.Completed:
                    disableAllInput = true;
                    currentStep = 0;
                    break;
            }

            InitRiskMatrixDictionary();
            await this.LoadSCECodeAsync();
            await this.LoadUserDataAsync();

            //Person person = await UserServiceClient.Person_GetPersonAsync(GlobalDataSource.User.Email);
            //Console.WriteLine(person.Department);
            //Console.WriteLine(await UserServiceClient.Department_GetDepartmentsAsync());
        }
        private async Task LoadUserDataAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<User>? userService = this.GetGenericService(serviceScope);
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
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<SCEElementRecord>? sceService = this.GetGenericSCEService(serviceScope);
            var query = sceService.Get();

            Microsoft.OData.Client.QueryOperationResponse<SCEElementRecord>? sceResponse = await query
                .OrderBy(x => x.SCECode)
                .ToQueryOperationResponseAsync<SCEElementRecord>();

            sceCodes = sceResponse
                .Select(x => x.SCECode)
                .ToList();

            StateHasChanged();
        }
        private async Task OnLMAutoCompleteSelectionChange(AutoCompleteOption item)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<User>? userService = this.GetGenericService(serviceScope);
            var query = userService.Get();

            Microsoft.OData.Client.QueryOperationResponse<User>? usersResponse = await query
                .Where(x => x.Role == item.Value.ToString())
                .OrderByDescending(x => x.Username)
                .ToQueryOperationResponseAsync<User>();

            lineManagerUsername = usersResponse
                .Select(x => x.Username)
                .ToList();

            StateHasChanged();
        }
        private async Task OnAAAutoCompleteSelectionChange(AutoCompleteOption item)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<User>? userService = this.GetGenericService(serviceScope);
            var query = userService.Get();

            Microsoft.OData.Client.QueryOperationResponse<User>? usersResponse = await query
                .Where(x => x.Role == item.Value.ToString())
                .OrderByDescending(x => x.Username)
                .ToQueryOperationResponseAsync<User>();

            approveAuthorityUsername = usersResponse
                .Select(x => x.Username)
                .ToList();

            StateHasChanged();
        }
        private async Task OnCloseOutAutoCompleteSelectionChange(AutoCompleteOption item)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<User>? userService = this.GetGenericService(serviceScope);
            var query = userService.Get();

            Microsoft.OData.Client.QueryOperationResponse<User>? usersResponse = await query
                .Where(x => x.Role == item.Value.ToString())
                .OrderByDescending(x => x.Username)
                .ToQueryOperationResponseAsync<User>();

            closeOutAuthorityUsername = usersResponse
                .Select(x => x.Username)
                .ToList();

            StateHasChanged();
        }
        private async Task OnExtensionAutoCompleteSelectionChange(AutoCompleteOption item, int currentExtensionNo)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<User>? userService = this.GetGenericService(serviceScope);
            var query = userService.Get();

            Microsoft.OData.Client.QueryOperationResponse<User>? usersResponse = await query
                .Where(x => x.Role == item.Value.ToString())
                .OrderByDescending(x => x.Username)
                .ToQueryOperationResponseAsync<User>();

            var userString = usersResponse
                .Select(x => x.Username)
                .ToList();

            Item.Extensions[currentExtensionNo - 1].ApproverNameColection = string.Empty;

            foreach (string user in userString)
            {
                Item.Extensions[currentExtensionNo - 1].ApproverNameColection += $"{user},";
            }

            StateHasChanged();
        }
        private void OnNextButtonClick()
        {
            if (Item.Identification.DetailOfChange == DetailOfChange.FacilitiesImprovementPlan)
            {
                currentStep = 4;
            }
            else
            {
                if (Item.ManagementOfChangeCurrentStep == ManagementOfChangeCurrentStep.RiskEvaluation)
                {
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
            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.New;

            if (Item.Identification.DetailOfChange == DetailOfChange.FacilitiesImprovementPlan)
            {
                Item.CloseOut.CloseOutState = CloseOutState.MadePermanentState;
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.CloseOutSubmitForApproval;
                currentStep = 4;
            }
            else
            {
                Item.CloseOut.CloseOutState = CloseOutState.RevertOriginalState;
                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.RiskEvaluation;
                currentStep = 1;
            }

            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericMOCService(serviceScope);

            await service.InsertAsync(Item);
            AffraNotificationService.NotifyItemCreated();
        }
        private void OnCloseDialogClicked()
        {
            DialogService.Close(false);
        }
        private async void OnEndorsementSubmitButtonClick()
        {
            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.EndorsementPendingForApproval;

            await SubmitAsync(Item);


            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<Message>? notificationService = this.GetGenericNotificationService(serviceScope);
            var query = notificationService.InsertAsync(new Message
            {
                Subject = Item.RecordNumber,
                Content = "Approve now!",
                Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.Endorsement.Name
                }
            });
        }
        private async void OnAuthorisationAndApprovalSubmitButtonClick()
        {
            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.AuthorisationAndApprovalPendingForApproval;

            await SubmitAsync(Item);
        }
        private async void OnExtensionSubmitButtonClick()
        {
            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Extension;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.ExtensionPendingForApproval;

            await SubmitAsync(Item);
        }
        private async void OnCloseOutSubmitButtonClick()
        {
            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.CloseoutPendingForApproval;

            await SubmitAsync(Item);
        }
        private void OnAddExtensionButtonClick()
        {
            Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Extension;
            Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.ExtensionSubmitForApproval;

            Item.Extensions.Add(new Extension
            {
                ExtensionNo = Item.Extensions.Count + 1,
                DurationExtended = 0,
                ReviewDate = System.DateTimeOffset.UtcNow,
                ApproverNameColection = string.Empty,
                ApproverName = string.Empty,
                ApproverDesignation = string.Empty,
                ApprovalSignature = string.Empty
            });
            disableAllInput = false;
            disableAddExtensionButton = true;
        }
        private void OnStepChange(int current)
        {
            this.currentStep = current;
        }
        protected Task SubmitAsync(ManagementOfChangeRecord args)
        {
            DialogService.Close(true);
            return Task.CompletedTask;
        }
        private void OnIdentificationExpiryDateChange(DateTimeChangedEventArgs args)
        {
            Item.Identification.DurationOfChange = (int)(args.Date - DateTime.Now.AddDays(-1)).TotalDays;
        }
        private void OnMOCFieldChange(ManagementOfChangeField args)
        {
            Item.RecordNumber = $"MOC-{args}-{DateTime.Now:yyyyMMddHHmmss}";
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
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Completed;
                Item.CloseOut.DateUI = DateTime.Now;
                await SubmitAsync(Item);
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
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
                Item.CloseOut.DateUI = DateTime.Now;
                await SubmitAsync(Item);
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
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
                Item.Endorsement.Signature = Item.Endorsement.Name;
                Item.CloseOut.DateUI = DateTime.Now;
                await SubmitAsync(Item);
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
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
                Item.CloseOut.DateUI = DateTime.Now;
                await SubmitAsync(Item);
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
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
                Item.AuthorisationAndApproval.Signature = Item.AuthorisationAndApproval.Name;
                Item.CloseOut.DateUI = DateTime.Now;
                await SubmitAsync(Item);
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
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
                Item.CloseOut.DateUI = DateTime.Now;
                await SubmitAsync(Item);
            }
        }
        private async Task ShowExtensionConfirmationDialogAsync(Extension item)
        {
            var content = $"Are you sure want to approve record no #{Item.RecordNumber} ?";
            var title = $"Management of Change - Extension #{item.ExtensionNo}";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.Extensions[item.ExtensionNo - 1].ApprovalSignature = item.ApproverName;

                Item.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.CloseOutSubmitForApproval;
                Item.ManagementOfChangeStatus = ManagementOfChangeStatus.Pending;
                Item.CloseOut.DateUI = DateTime.Now;
                await SubmitAsync(Item);
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
                await SubmitAsync(Item);
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
