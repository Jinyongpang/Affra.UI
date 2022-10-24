﻿using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class ManagementOfChangeDataList
    {
        private AntList<ManagementOfChangeRecord> _managementOfChangeList;
        private List<ManagementOfChangeRecord> managementOfChanges;
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
        [Inject] private ConfirmService ConfirmService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        [Inject] private IUserService UserService { get; set; }

        public CommonFilter ManagementOfChangeFilter { get; set; }

        private readonly Func<double, string> _fortmat1 = (p) => $"{p} %";
        private readonly ListGridType grid = new()
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
            ManagementOfChangeFilter = new CommonFilter(navigationManager);
            initLoading = false;
            return LoadDataAsync();
        }
        private async Task LoadDataAsync(bool isLoadMore = false)
        {
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.ManagementOfChange), HasReadPermissoin = true, HasWritePermission = true});
            isLoading = true;
            if (!isLoadMore)
            {
                currentCount = 0;
            }

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<ManagementOfChangeRecord>? managementOfChangeService = GetGenericMOCService(serviceScope);
            var query = managementOfChangeService.Get();

            if (!string.IsNullOrEmpty(ManagementOfChangeFilter.Search))
            {
                query = query
                    .Where(mocRecord => mocRecord.TitleOfChange.ToUpper().Contains(ManagementOfChangeFilter.Search.ToUpper())
                        || mocRecord.CreatedBy.ToUpper().Contains(ManagementOfChangeFilter.Search.ToUpper())
                        || mocRecord.RecordNumber.ToUpper().Contains(ManagementOfChangeFilter.Search.ToUpper())
                    );
            }
             
            if (ManagementOfChangeFilter.Status != null)
            {
                var status = (ManagementOfChangeStatus)Enum.Parse(typeof(ManagementOfChangeStatus), ManagementOfChangeFilter.Status);
                query = query.Where(mocRecord => mocRecord.ManagementOfChangeStatus == status);
            }
            else
            {
                query = query.Where(mocRecord => mocRecord.ManagementOfChangeStatus != ManagementOfChangeStatus.Deleted);
            }

            Microsoft.OData.Client.QueryOperationResponse<ManagementOfChangeRecord>? managementOfChangeResponse = await query
                .OrderByDescending(moc => moc.CreatedDateTime)
                .Skip(currentCount)
                .Take(loadSize)
                .ToQueryOperationResponseAsync<ManagementOfChangeRecord>();

            count = (int)managementOfChangeResponse.Count;
            currentCount += loadSize;
            var managementOfChangeList = managementOfChangeResponse.ToList();

            if (isLoadMore)
            {
                managementOfChanges.AddRange(managementOfChangeList);
            }
            else
            {
                managementOfChanges = managementOfChangeList;
            }

            isLoading = false;

            if (managementOfChangeList.DistinctBy(x => x.Id).Count() != managementOfChangeList.Count)
            {
                AffraNotificationService.NotifyWarning("Data have changed. Kindly reload.");
            }

            StateHasChanged();
        }
        public Task ReloadAsync()
        {
            return LoadDataAsync();
        }
        public Task OnLoadMoreAsync()
        {
            return LoadDataAsync(true);
        }
        private async Task ShowDialogAsync(ManagementOfChangeRecord data, string title = "")
        {
            dynamic? response;
            response = await DialogService.OpenAsync<MOCFormTemplate>(title,
                       new Dictionary<string, object>() { { "item", data } },
                       Constant.MOHDialogOptions);

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = GetGenericMOCService(serviceScope);

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
        private IGenericService<ManagementOfChangeRecord> GetGenericMOCService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<ManagementOfChangeRecord, IManagementOfChangeUnitOfWork>>();
        }
        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }
        private int GetPercentage(double step)
        {
            double enumLength = Enum.GetNames(typeof(ManagementOfChangeCurrentStep)).Length - 1;
            return (int)(step / enumLength * 100);
        }
        private async Task DeleteMOCFormAsync(ManagementOfChangeRecord data)
        {
            var content = $"Are you sure want to delete record no #{data.RecordNumber} ?";
            var title = "Management of Change - Deletion";

            var confirmResult = await ConfirmService.Show(content, title, ConfirmButtons.YesNo, ConfirmIcon.Question);

            if (confirmResult == ConfirmResult.Yes)
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var service = GetGenericMOCService(serviceScope);

                data.ManagementOfChangeStatus = ManagementOfChangeStatus.Deleted;
                data.ManagementOfChangeCurrentStep = ManagementOfChangeCurrentStep.Completed;

                await service.UpdateAsync(data, data.Id);
                AffraNotificationService.NotifyItemDeleted();
            }

            await ReloadAsync();
        }
    }
}
