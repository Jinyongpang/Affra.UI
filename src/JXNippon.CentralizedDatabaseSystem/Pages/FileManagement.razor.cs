using Affra.Core.Domain.Services;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class FileManagement
    {
        private DateTime? date = null;
        private RadzenDataList<DataFile> _dataList;
        private IEnumerable<DataFile> files;
        private int count;
        private string search = string.Empty;
        private bool isLoading = false;
        private FileProcessStatus? fileProcessStatus = null;
        private string status = null;
        private IEnumerable<string> fileProcessStatuses = Enum.GetValues(typeof(FileProcessStatus))        
            .Cast<FileProcessStatus>()
            .Select(x => x.ToString())
            .ToList();

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        protected override void OnInitialized()
        {
            search = NavManager.GetQueryString<string>(nameof(search));
            date = NavManager.GetQueryString<DateTime?>(nameof(date));
            status = NavManager.GetQueryString<string>(nameof(fileProcessStatus));
            if (!string.IsNullOrEmpty(status))
            {
                fileProcessStatus = (FileProcessStatus)Enum.Parse(typeof(FileProcessStatus), status);
            }
        }
            

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            AppendQuery();
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<DataFile>? fileService = this.GetGenericFileService(serviceScope);
            var query = fileService.Get();
            if (!string.IsNullOrEmpty(search))
            { 
                query = query.Where(dataFile => dataFile.FileName.ToUpper().Contains(search.ToUpper()));
            }
            if (fileProcessStatus != null)
            {
                query = query.Where(dataFile => dataFile.ProcessStatus == fileProcessStatus);
            }
            if (date != null)
            {
                var start = TimeZoneInfo.ConvertTimeToUtc(date.Value);
                var end = TimeZoneInfo.ConvertTimeToUtc(date.Value.AddDays(1));
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

        private async Task OnChangeAsync(string value, string name)
        {
            search = value;
            await _dataList.Reload();
        }
        private async Task OnChangeStatusFilterAsync(object value, string name)
        {
            fileProcessStatus = value is null
                ? null
                : (FileProcessStatus)Enum.Parse(typeof(FileProcessStatus), value.ToString());
            await _dataList.Reload();
        }

        private void OnTodayClick()
        {
            date = DateTime.Now;
        }

        private async Task OnChangeAsync(DateTime? value, string name, string format)
        {
            date = value;
            await _dataList.Reload();
        }

        private void AppendQuery()
        {
            var queries = new Dictionary<string, string>();
            if (search != null)
            {
                queries.Add(nameof(search), search);
            }
            if (fileProcessStatus != null)
            {
                queries.Add(nameof(fileProcessStatus), fileProcessStatus.ToString());
            }
            if (date != null)
            {
                queries.Add(nameof(date), date.Value.ToString("yyyy-MM-dd"));
            }
            var uriBuilder = new UriBuilder(NavManager.Uri);
            NavManager.NavigateTo(QueryHelpers.AddQueryString(uriBuilder.Uri.AbsolutePath, queries));
        }
    }
}
