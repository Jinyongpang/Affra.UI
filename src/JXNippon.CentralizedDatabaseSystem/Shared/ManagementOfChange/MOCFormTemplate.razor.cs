using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.CloseOuts;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Identifications;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.OData.UriParser;
using Radzen;
using UserODataService.Affra.Service.User.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using Microsoft.OData.Edm;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class MOCFormTemplate
    {
        [Parameter] public ManagementOfChangeRecord Item { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private ConfirmService ConfirmService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        private List<string> userDesignation;
        private List<string> lineManagerUsername;
        private List<string> approveAuthorityUsername;
        private List<string> closeOutAuthorityUsername;
        private int currentStep;
        private bool disableAllInput;
        private bool disableAddExtensionButton;
        protected override Task OnInitializedAsync()
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

            return this.LoadUserDataAsync();
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
        private IGenericService<ManagementOfChangeRecord> GetGenericMOCService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<ManagementOfChangeRecord, IManagementOfChangeUnitOfWork>>();
        }
    }
}
