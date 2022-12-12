using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFolders;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Domain.Workspaces;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.FileManagement
{
    public partial class FileManagementDataList
    {
        private const int loadSize = 9;
        private Virtualize<DataFile> virtualize;
        private int count;
        private bool isLoading = false;
        private bool initLoading = true;
        private bool isUserHavePermission = true;
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
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IWorkspaceService WorkspaceService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        public CommonFilter FileManagementFilter { get; set; }
        public IEnumerable<string> FileProcessStatusFilters { get; set; }
        public string Folder { get; set; }

        protected async override Task OnInitializedAsync()
        {
            isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.Administration), HasReadPermissoin = true, HasWritePermission = true });
            this.FileManagementFilter = new CommonFilter(navigationManager);
        }

        public async Task ReloadAsync()
        {
            await this.virtualize.RefreshDataAsync();
            this.StateHasChanged();
        }

        private async ValueTask<ItemsProviderResult<DataFile>> LoadDataAsync(ItemsProviderRequest request)
        {
            try
            {
                isLoading = true;
                StateHasChanged();
                try
                {
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
                    if (FileProcessStatusFilters != null && FileProcessStatusFilters.Any())
                    {
                        query = query.Where(dataFile => FileProcessStatusFilters.Contains(dataFile.ProcessStatus.ToString()));
                    }
                    if (FileManagementFilter.Date != null)
                    {
                        var start = TimeZoneInfo.ConvertTimeToUtc(FileManagementFilter.Date.Value);
                        var end = TimeZoneInfo.ConvertTimeToUtc(FileManagementFilter.Date.Value.AddDays(1));
                        query = query
                            .Where(dataFile => dataFile.LastModifiedDateTime >= start)
                            .Where(dataFile => dataFile.LastModifiedDateTime < end);
                    }

                    if (!string.IsNullOrEmpty(Folder))
                    {
                        query = query
                            .Where(x => x.FolderName.ToUpper() == Folder.ToUpper());
                    }

                    Microsoft.OData.Client.QueryOperationResponse<DataFile>? filesResponse = await query
                        .OrderByDescending(file => file.LastModifiedDateTime)
                        .Skip(request.StartIndex)
                        .Take(request.Count)
                        .ToQueryOperationResponseAsync<DataFile>();

                    count = (int)filesResponse.Count;
                    var filesList = filesResponse.ToList();

                    isLoading = false;
                    return new ItemsProviderResult<DataFile>(filesList, count);
                }
                finally
                {
                    initLoading = false;
                    isLoading = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }

            return new ItemsProviderResult<DataFile>(Array.Empty<DataFile>(), count);
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private IGenericService<DataFile> GetGenericFileService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DataFile, IDataExtractorUnitOfWork>>();
        }

        private async Task DownloadAsync(DataFile dataFile)
        {
            try
            { 
                var folder = this.GetFolderName(dataFile.FolderName);
                var id = await this.WorkspaceService.GetIdAsync(folder, dataFile.FileName);
                var fileResponse = await this.WorkspaceService.DownloadAsync(id);
                if (fileResponse != null)
                {
                    using var streamRef = new DotNetStreamReference(stream: fileResponse.Stream);
                    await JSRuntime.InvokeVoidAsync("downloadFileFromStream", dataFile.FileName, streamRef);
                }
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }

        }

        private string GetFolderName(string value)
        {
            return value.Split('\\').Last();
        }

        private async Task ResyncFileAsync(DataFile dataFile)
        {
            var currentAttempts = dataFile.NumberOfAttempts;
            var currentStatus = dataFile.ProcessStatus;
            try
            {
                using var serviceScope = ServiceProvider.CreateScope();
                IGenericService<DataFile>? fileService = this.GetGenericFileService(serviceScope);
                dataFile.NumberOfAttempts = 0;
                dataFile.ProcessStatus = FileProcessStatus.Pending;
                await fileService.UpdateAsync(dataFile, dataFile.Id);

                AffraNotificationService.NotifySuccess("Reset to pending succeeded.");
            }
            catch (Exception ex)
            {
                dataFile.NumberOfAttempts = currentAttempts;
                dataFile.ProcessStatus = currentStatus;
                AffraNotificationService.NotifyException(ex);
            }          
        }
    }
}
