using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Localization;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.ManagementOfChange
{
    public partial class ManagementOfChangeDataList
    {
        private Virtualize<ManagementOfChangeRecord> virtualize;
        private List<ManagementOfChangeRecord> managementOfChanges;
        private RadzenDataGrid<ManagementOfChangeRecord> grid;
        private int count;
        private int currentCount;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
        private const int loadSize = 9;
        private IEnumerable<ManagementOfChangeRecord> items;
        [Parameter] public string DisplayType { get; set; } = "Card";
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private ConfirmService ConfirmService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }

        public CommonFilter ManagementOfChangeFilter { get; set; }

        private readonly Func<double, string> _fortmat1 = (p) => $"{p} %";

        protected override Task OnInitializedAsync()
        {
            ManagementOfChangeFilter = new CommonFilter(navigationManager);
            initLoading = false;
            return Task.CompletedTask;
        }
        private async Task LoadDataAsync(LoadDataArgs args)
        {
            try
            {
                isLoading = true;
                using var serviceScope = ServiceProvider.CreateScope();
                var query = this.GetManagementOfChangeQuery(serviceScope);

                Microsoft.OData.Client.QueryOperationResponse<ManagementOfChangeRecord>? response = await query
                    .AppendQuery(args.Filters, args.Skip, args.Top, args.Sorts)
                    .ToQueryOperationResponseAsync<ManagementOfChangeRecord>();

                count = (int)response.Count;
                items = response.ToList();
                isLoading = false;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private IQueryable<ManagementOfChangeRecord> GetManagementOfChangeQuery(IServiceScope serviceScope)
        {
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
            return query;

        }

        private async ValueTask<ItemsProviderResult<ManagementOfChangeRecord>> LoadDataAsync(ItemsProviderRequest request)
        {
            isLoading = true;
            StateHasChanged();

            try
            {
                isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.ManagementOfChange), HasReadPermissoin = true, HasWritePermission = true });
                using var serviceScope = ServiceProvider.CreateScope();
                var query = this.GetManagementOfChangeQuery(serviceScope);

                Microsoft.OData.Client.QueryOperationResponse<ManagementOfChangeRecord>? managementOfChangeResponse = await query
                    .OrderByDescending(moc => moc.CreatedDateTime)
                    .Skip(request.StartIndex)
                    .Take(request.Count)
                    .ToQueryOperationResponseAsync<ManagementOfChangeRecord>();

                count = (int)managementOfChangeResponse.Count;
                currentCount += loadSize;
                var managementOfChangeList = managementOfChangeResponse.ToList();

                isLoading = false;

                if (managementOfChangeList.DistinctBy(x => x.Id).Count() != managementOfChangeList.Count)
                {
                    AffraNotificationService.NotifyWarning("Data have changed. Kindly reload.");
                }

                return new ItemsProviderResult<ManagementOfChangeRecord>(managementOfChangeList, count);
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
            finally
            {
                initLoading = false;
                isLoading = false;
                StateHasChanged();
            }
        }
        public async Task ReloadAsync()
        {
            await this.virtualize.RefreshDataAsync();
            this.StateHasChanged();
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
