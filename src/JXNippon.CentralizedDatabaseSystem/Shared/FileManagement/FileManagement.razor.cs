using System;
using Affra.Core.Domain.Services;
using AntDesign;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFolders;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Domain.Workspaces;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.OData.Extensions.Client;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.FileManagement
{
    public class FileProcessStatusHolder
    {
        public FileProcessStatus FileProcessStatus {get; set;}

        public string FileProcessStatusString { get; set; }
    }
    public class RootFolder
    {
        public IEnumerable<DataFolder> Folders { get; set; }
    }
    public class SectionFolder
    {
        public string FolderName { get; set; }
    }
    public partial class FileManagement
    {
        private const string All = "All";
        private static readonly SectionFolder[] Folders = new string[]
        {
            "FPSODailyReport",
            "FPSOLabDailyReport",
            "ProcessDailyReport",
            "InstrumentDailyReport",
            "PIMSAndMETReport",
            "DailyGasProductionDeliverySchedule",
            "PEMonthlyReport",
            "VendorReport",
            "DefermentDetail",
            "WellTestReport",
            "FPSOProductionDBReport",
            "CondensateStreamReport",
            "DailyGasMeteringReport",
            "Templates",
            "AvailabilityAndReliabilityReport",
        }
            .OrderBy(x => x)
            .Select(x => new SectionFolder()
            {
                FolderName = x,
            })
            .ToArray();

        private FileManagementDataList fileManagementDataList;
        private readonly Menu menu;
        private string search;
        private string folder;
        private IEnumerable<RootFolder> folders; 
        private IEnumerable<string> selectedStatuses = new List<string>();
        private static IEnumerable<string> FileProcessStatusHolders = 
            Enum.GetValues(typeof(FileProcessStatus))
               .Cast<FileProcessStatus>()
               .Select(x => x.ToString())
               .ToList();

        [Inject] private IWorkspaceService WorkspaceService { get; set; }

        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var rootFolder = new RootFolder();
            folders = new List<RootFolder>()
            {
                rootFolder,
            };
            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<DataFolder>? folderService = GetGenericFolderService(serviceScope);
            var query = folderService.Get();
            rootFolder.Folders = (await folderService.Get().ExecuteAsync<DataFolder>()).ToList();
            await base.OnInitializedAsync();
        }
        private Task OnStatusFilterChangedAsync(object value)
        {
            this.selectedStatuses = value as IEnumerable<string>;
            return this.ReloadAsync();
        }
        private void LoadFolder(TreeExpandEventArgs args)
        {
            if (args.Value is RootFolder rootFolder)
            {
                args.Children.Data = rootFolder.Folders;
                args.Children.Text = GetTextForNode;
                args.Children.HasChildren = HasChildren;
                args.Children.Template = FolderTemplate;
            }
            else if (args.Value is DataFolder dataFolder)
            {
                args.Children.Data = dataFolder.Folders
                    .OrderBy(x => x.FolderName);
                args.Children.Text = GetTextForNode;
                args.Children.HasChildren = HasChildren;
                args.Children.Template = FolderTemplate;
            }
            else if (args.Value is Folder folder)
            {
                args.Children.Data = folder.Folders
                    .OrderBy(x => x.FolderName);
                args.Children.Text = GetTextForNode;
                args.Children.HasChildren = HasChildren;
                args.Children.Template = FolderTemplate;
            }
        }

        private async Task OnValueChangedAsync(object value)
        {
            string folderName = string.Empty;
            if (value is DataFolder dataFolder)
            {
                folderName = dataFolder.Folder;

            }
            else if (value is Folder folder)
            {
                folderName = folder.Folder1;
            }

            folder = folderName;
            await ReloadAsync();
        }

        private bool HasChildren(object data)
        {
            if (data is RootFolder rootFolder)
            {
                return rootFolder.Folders?.Count() > 0;
            }
            else if (data is DataFolder dataFolder)
            {
                return dataFolder.Folders?.Count > 0;
            }
            else if (data is Folder folder)
            {
                return folder.Folders?.Count > 0;
            }
            return false;
        }
        private readonly RenderFragment<RadzenTreeItem> FolderTemplate = (context) => builder =>
        {
            builder.OpenComponent<RadzenIcon>(0);
            builder.AddAttribute(1, nameof(RadzenIcon.Icon), "folder");
            builder.CloseComponent();
            builder.AddContent(3, context.Text);
        };
        private string GetTextForNode(object data)
        {
            if (data is RootFolder)
            {
                return "root";
            }
            else if (data is DataFolder dataFolder)
            {
                return dataFolder.Section;
            }
            else if (data is Folder folder)
            {
                return folder.FolderName;
            }
            return string.Empty;
        }

        private IGenericService<DataFolder> GetGenericFolderService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DataFolder, IDataExtractorUnitOfWork>>();
        }

        private Task ReloadAsync(string status = null, string searchInput = null)
        {
            //fileManagementDataList.FileManagementFilter.Status = GetStatusFilter(status);
            fileManagementDataList.FileManagementFilter.Search = searchInput ?? search;
            fileManagementDataList.Folder = folder;
            fileManagementDataList.FileProcessStatusFilters = this.selectedStatuses;
            return fileManagementDataList.ReloadAsync();
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
            return ReloadAsync(menuItem.Key);
        }

        private async Task UploadAsync(InputFileChangeEventArgs e)
        {
            try
            {
                foreach (var file in e.GetMultipleFiles(30))
                {
                    await WorkspaceService.UploadAsync(folder, new Domain.WorkspaceAPI.FileParameter(file.OpenReadStream(512000000), file.Name));
                }
                AffraNotificationService.NotifySuccess("File uploaded!");
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }
    }
}
