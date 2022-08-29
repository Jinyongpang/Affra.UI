using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.FileManagement
{
    public partial class FileManagementDataList
    {
        private const int loadSize = 9;
        private AntList<DataFile> _dataList;
        private List<DataFile> files;
        private int count;
        private int currentCount;
        private bool isLoading = false;
        private bool initLoading = true;
        private ListGridType grid = new()
        {
            Gutter = 16,
            Xs = 1,
            Sm = 1,
            Md = 1,
            Lg = 2,
            Xl = 3,
            Xxl = 3,
        };

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        public CommonFilter FileManagementFilter { get; set; }

        protected override Task OnInitializedAsync()
        {
            this.FileManagementFilter = new CommonFilter(navigationManager);
            initLoading = false;
            return this.LoadDataAsync();
        }

        public Task ReloadAsync()
        {
            return this.LoadDataAsync();
        }

        public Task OnLoadMoreAsync()
        {
            return this.LoadDataAsync(true);
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
            IGenericService<DataFile>? fileService = this.GetGenericFileService(serviceScope);
            var query = fileService.Get();
            if (!string.IsNullOrEmpty(FileManagementFilter.Search))
            {
                query = query.Where(dataFile => dataFile.FileName.ToUpper().Contains(FileManagementFilter.Search.ToUpper()));
            }
            if (FileManagementFilter.Status != null)
            {
                var status = (FileProcessStatus)Enum.Parse(typeof(FileProcessStatus), FileManagementFilter.Status);
                query = query.Where(dataFile => dataFile.ProcessStatus == status);
            }
            if (FileManagementFilter.Date != null)
            {
                var start = TimeZoneInfo.ConvertTimeToUtc(FileManagementFilter.Date.Value);
                var end = TimeZoneInfo.ConvertTimeToUtc(FileManagementFilter.Date.Value.AddDays(1));
                query = query
                    .Where(dataFile => dataFile.LastModifiedDateTime >= start)
                    .Where(dataFile => dataFile.LastModifiedDateTime < end);
            }
            

            Microsoft.OData.Client.QueryOperationResponse<DataFile>? filesResponse = await query
                .OrderByDescending(file => file.LastModifiedDateTime)
                .Skip(currentCount)
                .Take(loadSize)
                .ToQueryOperationResponseAsync<DataFile>();

            count = (int)filesResponse.Count;
            currentCount += loadSize;
            var filesList = filesResponse.ToList();

            if (isLoadMore)
            {
                files.AddRange(filesList);
            }
            else
            { 
                files = filesList;
            }

            isLoading = false;

            if (filesList.DistinctBy(x => x.Id).Count() != filesList.Count)
            {
                AffraNotificationService.NotifyWarning("Data have changed. Kindly reload.");
            }

            StateHasChanged();
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<DataFile> GetGenericFileService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DataFile, IDataExtractorUnitOfWork>>();
        }

        public async Task ResyncFile(DataFile dataFile)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<DataFile>? fileService = this.GetGenericFileService(serviceScope);
            dataFile.NumberOfAttempts = 0;
            dataFile.ProcessStatus = FileProcessStatus.Pending;
            await fileService.UpdateAsync(dataFile, dataFile.Id);
        }
    }
}
