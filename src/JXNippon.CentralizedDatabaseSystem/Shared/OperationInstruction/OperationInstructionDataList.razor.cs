using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.OperationInstructions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.OperationInstruction
{
    public partial class OperationInstructionDataList
    {
        private AntList<OperationInstructionRecord> _operationInstructionList;
        private List<OperationInstructionRecord> operationInstructions;
        private int count;
        private int currentCount;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
        private const int loadSize = 9;
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        [Inject] private ConfirmService ConfirmService { get; set; }
        [Inject] private IUserService UserService { get; set; }

        public CommonFilter OperationInstructionFilter { get; set; }

        private Func<double, string> _fortmat1 = (p) => $"{p} %";
        private ListGridType grid = new()
        {
            Gutter = 16,
            Xs = 1,
            Sm = 1,
            Md = 1,
            Lg = 3,
            Xl = 3,
            Xxl = 3,
        };
        protected override Task OnInitializedAsync()
        {
            this.OperationInstructionFilter = new CommonFilter(navigationManager);
            initLoading = false;
            return this.LoadDataAsync();
        }
        private async Task LoadDataAsync(bool isLoadMore = false)
        {
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.ManagementOfChange), HasReadPermissoin = true, HasWritePermission = true });
            isLoading = true;
            StateHasChanged();
            if (!isLoadMore)
            {
                currentCount = 0;
            }

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<OperationInstructionRecord>? operationInstructionService = this.GetGenericOIService(serviceScope);
            var query = operationInstructionService.Get();

            if (!string.IsNullOrEmpty(OperationInstructionFilter.Search))
            {
                query = query
                    .Where(oiRecord => oiRecord.CreatedBy.ToUpper().Contains(OperationInstructionFilter.Search.ToUpper())
                        || oiRecord.OperationInstructionNo.ToUpper().Contains(OperationInstructionFilter.Search.ToUpper())
                    );
            }
            if (OperationInstructionFilter.Status != null)
            {
                var status = (OperationInstructionStatus)Enum.Parse(typeof(OperationInstructionStatus), OperationInstructionFilter.Status);
                query = query.Where(oiRecord => oiRecord.OperationInstructionStatus == status);
            }
            else
            {
                query = query.Where(oiRecord => oiRecord.OperationInstructionStatus != OperationInstructionStatus.Deleted);
            }

            Microsoft.OData.Client.QueryOperationResponse<OperationInstructionRecord>? operationInstructionResponse = await query
                .OrderByDescending(oi => oi.CreatedDateTime)
                .Skip(currentCount)
                .Take(loadSize)
                .ToQueryOperationResponseAsync<OperationInstructionRecord>();

            count = (int)operationInstructionResponse.Count;
            currentCount += loadSize;
            var operationInstructionList = operationInstructionResponse.ToList();

            if (isLoadMore)
            {
                operationInstructions.AddRange(operationInstructionList);
            }
            else
            {
                operationInstructions = operationInstructionList;
            }

            isLoading = false;

            if (operationInstructionList.DistinctBy(x => x.Id).Count() != operationInstructionList.Count)
            {
                AffraNotificationService.NotifyWarning("Data have changed. Kindly reload.");
            }

            StateHasChanged();
        }
        public Task ReloadAsync()
        {
            return this.LoadDataAsync();
        }
        public Task OnLoadMoreAsync()
        {
            return this.LoadDataAsync(true);
        }
        private async Task ShowDialogAsync(OperationInstructionRecord data, string title = "")
        {
            dynamic? response;
            response = await DialogService.OpenAsync<OperationInstructionFormTemplate>(title,
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
                        isLoading = true;
                        await service.UpdateAsync(data, data.Id);
                        AffraNotificationService.NotifyItemUpdated();
                    }
                    else
                    {
                        isLoading = true;
                        await service.InsertAsync(data);
                        AffraNotificationService.NotifyItemCreated();
                    }
                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
                finally
                {
                    isLoading = false;
                }
            }

            await ReloadAsync();
        }
        private IGenericService<OperationInstructionRecord> GetGenericOIService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<OperationInstructionRecord, IManagementOfChangeUnitOfWork>>();
        }
        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }
        private int GetPercentage(double step)
        {
            double enumLength = Enum.GetNames(typeof(OperationInstructionCurrentStep)).Length - 1;
            return (int)((step / enumLength) * 100);
        }
        private async Task DeleteOIFormAsync(OperationInstructionRecord data)
        {
            var content = $"Are you sure want to delete record no #{data.OperationInstructionNo} ?";
            var title = "Operation Instruction - Deletion";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var service = GetGenericOIService(serviceScope);

                data.OperationInstructionStatus = OperationInstructionStatus.Deleted;
                data.OperationInstructionCurrentStep = OperationInstructionCurrentStep.Completed;

                await service.UpdateAsync(data, data.Id);
                AffraNotificationService.NotifyItemDeleted();
            }

            await ReloadAsync();
        }
    }
}
