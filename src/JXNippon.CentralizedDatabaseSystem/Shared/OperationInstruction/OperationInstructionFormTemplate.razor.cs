using Affra.Core.Domain.Services;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Domain.Notifications;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.OperationInstructions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using OpenAPI.UserService;
using Radzen;
using Message = NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages.Message;
using User = UserODataService.Affra.Service.User.Domain.Users.User;

namespace JXNippon.CentralizedDatabaseSystem.Shared.OperationInstruction
{
    public partial class OperationInstructionFormTemplate
    {
        [Parameter] public OperationInstructionRecord Item { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IUserServiceClient UserServiceClient { get; set; }
        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }
        [Inject] private ConfirmService ConfirmService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }

        private List<string> EndorserUsername;
        private List<string> approvalUsername;
        private int currentStep;
        private bool disableAllInput;
        protected async override Task OnInitializedAsync()
        {
            disableAllInput = false;

            switch (Item.OperationInstructionCurrentStep)
            {
                case OperationInstructionCurrentStep.InitialCreation:
                    currentStep = 0;
                    break;
                case OperationInstructionCurrentStep.EndorsementPendingForApproval:
                    disableAllInput = true;
                    currentStep = 1;
                    break;
                case OperationInstructionCurrentStep.EndorsementSubmitForApproval:
                    currentStep = 1;
                    await LoadEndoserUsernames();
                    break;
                case OperationInstructionCurrentStep.ApprovalPendingForApproval:
                    disableAllInput = true;
                    currentStep = 2;
                    break;
                case OperationInstructionCurrentStep.ApprovalSubmitForApproval:
                    currentStep = 2;
                    await LoadApprovalUsernames();
                    break;
                case OperationInstructionCurrentStep.Completed:
                    disableAllInput = true;
                    currentStep = 0;
                    break;
            }
        }
        private async Task LoadEndoserUsernames()
        {
            Person person = await UserServiceClient.Person_GetPersonAsync(GlobalDataSource.User.Email);

            EndorserUsername = new List<string>();

            foreach (string manager in await UserServiceClient.Manager_GetManagersAsync(person.Department))
            {
                EndorserUsername.Add(manager);

                foreach (string delegation in await UserServiceClient.Delegation_GetDelegationsAsync(manager, person.Department))
                {
                    EndorserUsername.Add(delegation);
                }
            }

            EndorserUsername = EndorserUsername.Distinct().ToList();

            StateHasChanged();
        }
        private async Task LoadApprovalUsernames()
        {
            Person person = await UserServiceClient.Person_GetPersonAsync(GlobalDataSource.User.Email);

            approvalUsername = new List<string>();

            foreach (string manager in await UserServiceClient.Manager_GetManagersAsync(person.Department))
            {
                approvalUsername.Add(manager);

                foreach (string delegation in await UserServiceClient.Delegation_GetDelegationsAsync(manager, person.Department))
                {
                    approvalUsername.Add(delegation);
                }
            }

            approvalUsername = approvalUsername.Distinct().ToList();

            StateHasChanged();
        }
        private void OnStepChange(int current)
        {
            this.currentStep = current;
        }
        protected Task SubmitAsync(OperationInstructionRecord args)
        {
            DialogService.Close(true);
            return Task.CompletedTask;
        }
        private void OnEstimatedDurationChange(int args)
        {
            Item.EstimatedDurationDateTimeUI = DateTime.Now.AddDays(args);
        }
        private async void OnCreateButtonClick()
        {
            if (Item.EstimatedDuration <= 0)
            {
                AffraNotificationService.NotifyWarning("Estimated duration cannot be less than or equal 0.");
                return;
            }

            Item.OperationInstructionStatus = OperationInstructionStatus.New;

            Item.OperationInstructionCurrentStep = OperationInstructionCurrentStep.EndorsementSubmitForApproval;
            currentStep = 1;

            Item.OperationInstructionNo = $"OI-{Item.OperationInstructionField.ToString()}-{DateTime.Now:yyyy}-{DateTime.Now:yyyyMMddHHmmss}";

            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.GetGenericOIService(serviceScope);

            await service.InsertAsync(Item);
            AffraNotificationService.NotifyItemCreated(); 
            
            await LoadEndoserUsernames();
        }
        private void OnCloseDialogClicked()
        {
            DialogService.Close(false);
        }
        private void OnNextButtonClick()
        {
            currentStep++;
        }
        private void OnPreviousButtonClick()
        {
            currentStep--;
        }
        private async void OnEndorsementSubmitButtonClick()
        {
            Item.OperationInstructionStatus = OperationInstructionStatus.PendingForApproval;
            Item.OperationInstructionCurrentStep = OperationInstructionCurrentStep.EndorsementPendingForApproval;

            await SubmitAsync(Item);

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<Message>? notificationService = this.GetGenericNotificationService(serviceScope);
            var query = notificationService.InsertAsync(new Message
            {
                Subject = Item.OperationInstructionNo,
                Content = "You have a new operation instruction to approve!",
                Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.EndorsedBy
                }
            });
        }
        private async void OnApprovalSubmitButtonClick()
        {
            Item.OperationInstructionStatus = OperationInstructionStatus.PendingForApproval;
            Item.OperationInstructionCurrentStep = OperationInstructionCurrentStep.ApprovalPendingForApproval;

            await SubmitAsync(Item);

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<Message>? notificationService = this.GetGenericNotificationService(serviceScope);
            var query = notificationService.InsertAsync(new Message
            {
                Subject = Item.OperationInstructionNo,
                Content = "You have a new operation instruction to approve!",
                Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.ApprovedBy
                }
            });
        }
        private async Task ShowEndorsementConfirmationDialogAsync()
        {
            var content = $"Are you sure want to approve record no #{Item.OperationInstructionNo} ?";
            var title = "Operation Instruction - Endorsement";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.OperationInstructionCurrentStep = OperationInstructionCurrentStep.ApprovalSubmitForApproval;
                Item.OperationInstructionStatus = OperationInstructionStatus.Approved;
                Item.EndorserSignature = Item.EndorsedBy;
                Item.EndorsedByDateTimeUI = DateTime.Now;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = this.GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.OperationInstructionNo,
                    Content = "Your operation instruction has been approved!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.PreparedBy
                }
                });
            }
        }
        private async Task ShowEndorsementRejectDialogAsync()
        {
            var content = $"Are you sure want to reject record no #{Item.OperationInstructionNo} ?";
            var title = "Operation Instruction - Endorsement";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.OperationInstructionCurrentStep = OperationInstructionCurrentStep.EndorsementSubmitForApproval;
                Item.OperationInstructionStatus = OperationInstructionStatus.Rejected;
                Item.EndorserSignature = string.Empty;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = this.GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.OperationInstructionNo,
                    Content = "Your operation instruction has been rejected!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.PreparedBy
                }
                });
            }
        }
        private async Task ShowApprovalConfirmationDialogAsync()
        {
            var content = $"Are you sure want to approve record no #{Item.OperationInstructionNo} ?";
            var title = "Operation Instruction - Approval";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.OperationInstructionCurrentStep = OperationInstructionCurrentStep.Completed;
                Item.OperationInstructionStatus = OperationInstructionStatus.Completed;
                Item.ApproverSignature = Item.ApprovedBy;
                Item.ApprovedByDateTimeUI = DateTime.Now;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = this.GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.OperationInstructionNo,
                    Content = "Your operation instruction has been approved!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.PreparedBy
                }
                });
            }
        }
        private async Task ShowApprovalRejectDialogAsync()
        {
            var content = $"Are you sure want to reject record no #{Item.OperationInstructionNo} ?";
            var title = "Operation Instruction - Approval";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                Item.OperationInstructionCurrentStep = OperationInstructionCurrentStep.ApprovalSubmitForApproval;
                Item.OperationInstructionStatus = OperationInstructionStatus.Rejected;
                Item.ApproverSignature = string.Empty;
                await SubmitAsync(Item);

                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<Message>? notificationService = this.GetGenericNotificationService(serviceScope);
                var query = notificationService.InsertAsync(new Message
                {
                    Subject = Item.OperationInstructionNo,
                    Content = "Your operation instruction has been rejected!",
                    Users = new System.Collections.ObjectModel.Collection<string> {
                    Item.PreparedBy
                }
                });
            }
        }
        private IGenericService<User> GetGenericUserService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<User, IUserUnitOfWork>>();
        }
        private IGenericService<Message> GetGenericNotificationService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<Message, INotificationUnitOfWork>>();
        }
        private IGenericService<OperationInstructionRecord> GetGenericOIService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<OperationInstructionRecord, IManagementOfChangeUnitOfWork>>();
        }
    }
}
