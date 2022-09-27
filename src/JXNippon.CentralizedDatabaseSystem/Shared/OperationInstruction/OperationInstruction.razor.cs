using Affra.Core.Domain.Services;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.OperationInstructions;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.OperationInstruction
{
    public partial class OperationInstruction
    {
        private const string All = "All";
        private OperationInstructionDataList operationInstructionDataList;
        private Menu menu;
        private string search;
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        private Task ReloadAsync(string status = null)
        {
            operationInstructionDataList.OperationInstructionFilter.Status = GetStatusFilter(status);
            operationInstructionDataList.OperationInstructionFilter.Search = search;
            return operationInstructionDataList.ReloadAsync();
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
            return this.ReloadAsync(menuItem.Key);
        }
        private async Task ShowDialogAsync(OperationInstructionRecord data, string title = "")
        {
            data = new()
            {
                OperationInstructionNo = string.Empty,
                OperationInstructionCurrentStep = OperationInstructionCurrentStep.InitialCreation,
                OperationInstructionStatus = OperationInstructionStatus.New,
                OperationInstructionField = OperationInstructionField.HIP,
                EstimatedDurationDateTimeUI = DateTime.Now,
                EndorserSignature = string.Empty,
                EndorsedByDateTimeUI = DateTime.Now,
                ApproverSignature = string.Empty,
                ApprovedByDateTimeUI = DateTime.Now,
                RevisionNo = 0
            };

            dynamic? response;
            response = await DialogService.OpenAsync<OperationInstructionFormTemplate>("",
                new Dictionary<string, object>() { { "item", data } },
                Constant.MOHDialogOptions);

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericOIService(serviceScope);

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
        private IGenericService<OperationInstructionRecord> GetGenericOIService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<OperationInstructionRecord, IManagementOfChangeUnitOfWork>>();
        }
    }
}