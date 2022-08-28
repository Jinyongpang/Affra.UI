using Microsoft.AspNetCore.Components;
using Radzen;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using AntDesign;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Models;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;

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
        private const int loadSize = 9;
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        public CommonFilter ManagementOfChangeFilter { get; set; }

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
            this.ManagementOfChangeFilter = new CommonFilter(navigationManager);
            initLoading = false;
            return this.LoadDataAsync();
        }
        private async Task LoadDataAsync(bool isLoadMore = false)
        {
            isLoading = true;
            StateHasChanged();
            if (!isLoadMore)
            {
                currentCount = 0;
            }

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<ManagementOfChangeRecord>? managementOfChangeService = this.GetGenericMOCService(serviceScope);
            var query = managementOfChangeService.Get();

            if (!string.IsNullOrEmpty(ManagementOfChangeFilter.Search))
            {
                query = query.Where(mocRecord => mocRecord.ManagementOfChangeStatus.ToString().ToUpper().Contains(ManagementOfChangeFilter.Search.ToUpper()));
            }
            if (ManagementOfChangeFilter.Status != null)
            {
                var status = (ManagementOfChangeStatus)Enum.Parse(typeof(ManagementOfChangeStatus), ManagementOfChangeFilter.Status);
                query = query.Where(mocRecord => mocRecord.ManagementOfChangeStatus == status);
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
            return this.LoadDataAsync();
        }
        public Task OnLoadMoreAsync()
        {
            return this.LoadDataAsync(true);
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
                    var service = this.GetGenericMOCService(serviceScope);

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
            return (int)((step / enumLength) * 100);
        }
    }
}
