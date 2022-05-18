using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using JXNippon.CentralizedDatabaseSystem.Models;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class FileManagementFilterPanel
    {
        private IEnumerable<string> fileProcessStatuses = Enum.GetValues(typeof(FileProcessStatus))
            .Cast<FileProcessStatus>()
            .Select(x => x.ToString())
            .ToList();
        [Parameter] public EventCallback<CommonFilter> Change { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        public CommonFilter CommonFilter { get; set; }

        protected override Task OnInitializedAsync()
        {
            CommonFilter = new CommonFilter(NavManager);
            return Task.CompletedTask;
        }
        private Task OnChangeAsync(object value)
        {
            CommonFilter.AppendQuery(NavManager);
            return Change.InvokeAsync(CommonFilter);
        }
    }
}
