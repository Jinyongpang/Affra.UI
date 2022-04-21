using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class FileManagement
    {
        private RadzenDataGrid<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File> grid;
        private IEnumerable<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File> files;
        private DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File fileToInsert;
        private int count;
        private bool isLoading;

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }

        private async Task Reload()
        {
            grid.Reset(true);
            await grid.Reload();
        }

        private async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;
            using var serviceScope = ServiceProvider.CreateScope();
            var fileManagementService = this.GetFileManagementService(serviceScope);
            var filesResponse = await fileManagementService.Get()
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File>();

            count = (int)filesResponse.Count;
            files = filesResponse.ToList();

            isLoading = false;
        }

        private async Task EditRow(DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File file)
        {
            await grid.EditRow(file);
        }

        private async Task SaveRow(DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File file)
        {
            try
            {
                if (file == fileToInsert)
                {
                    fileToInsert = null;
                }
                using var serviceScope = ServiceProvider.CreateScope();
                var fileService = this.GetGenericFileService(serviceScope);
                await fileService.UpdateAsync(file, file.Id);
                await grid.UpdateRow(file);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        private void CancelEdit(DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File file)
        {
            if (file == fileToInsert)
            {
                fileToInsert = null;
            }

            grid.CancelEditRow(file);

        }

        private async Task DeleteRow(DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File file)
        {
            try
            {
                if (file == fileToInsert)
                {
                    fileToInsert = null;
                }

                if (files.Contains(file))
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var fileService = this.GetGenericFileService(serviceScope);
                    await fileService.DeleteAsync(file);
                    await grid.Reload();
                }
                else
                {
                    grid.CancelEditRow(file);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
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

        private IGenericService<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File> GetGenericFileService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IGenericService<DataExtractorODataService.Affra.Service.DataExtractor.Domain.Files.File>>();
        }

        private IFileManagementService GetFileManagementService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IFileManagementService>();
        }
    }
}
