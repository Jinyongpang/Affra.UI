using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class FileManagementDataList
    {

        private RadzenDataList<DataFile> _dataList;
        private IEnumerable<DataFile> files;
        private int count;
        private bool isLoading = false;

        [Parameter] public EventCallback<LoadDataArgs> LoadData { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }

        public CommonFilter FileManagementFilter { get; set; }

        protected override void OnInitialized()
        {
        }

        public async Task ReloadAsync()
        {
            await _dataList.FirstPage(true);
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            await LoadData.InvokeAsync();
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
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DataFile>();

            count = (int)filesResponse.Count;
            files = filesResponse.ToList();
            isLoading = false;
        }

        private void HandleException(Exception ex)
        {
            NotificationService.Notify(new()
            {
                Summary = "Error",
                Detail = ex.InnerException?.ToString(),
                Severity = NotificationSeverity.Error,
                Duration = 120000,
            });
        }

        private IGenericService<DataFile> GetGenericFileService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DataFile, IDataExtractorUnitOfWork>>();
        }
    }
}
