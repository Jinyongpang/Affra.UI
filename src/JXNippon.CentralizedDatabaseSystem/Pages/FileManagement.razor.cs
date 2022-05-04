using Affra.Core.Domain.Services;
using DataExtractorODataService.Affra.Service.DataExtractor.Domain.DataFiles;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class FileManagement
    {
        private IEnumerable<DataFile> files;
        private DataFile fileToInsert;
        private int count;

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }

        private async Task LoadData(LoadDataArgs args)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var fileService = this.GetGenericFileService(serviceScope);
            var filesResponse = await fileService.Get()
                .AppendQuery(args.Filter, args.Skip, args.Top, args.OrderBy)
                .ToQueryOperationResponseAsync<DataFile>();

            count = (int)filesResponse.Count;
            files = filesResponse.ToList();
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
