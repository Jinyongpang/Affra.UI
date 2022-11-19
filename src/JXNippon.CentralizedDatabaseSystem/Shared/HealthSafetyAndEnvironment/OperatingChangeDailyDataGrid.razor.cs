using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.HealthSafetyEnvironments;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Shared.Commons;
using ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared.HealthSafetyAndEnvironment
{
    public partial class OperatingChangeDailyDataGrid<TDialog> : DailyDataGrid<DailyOperatingChange, TDialog>
        where TDialog : ComponentBase, IDailyDialog<DailyOperatingChange>
    {
		[Inject] private IServiceProvider ServiceProvider { get; set; }

		protected override async Task LoadDataAsync(LoadDataArgs args)
        {
			isLoading = true;
			await LoadData.InvokeAsync();
			using var serviceScope = ServiceProvider.CreateScope();
			var service = GetGenericMOCService(serviceScope);
			var query = service.Get();
			query = query
				.Where(x => x.CreatedDateTime >= ReportDate)
				.Where(x => x.Identification.ExpiryDate <= ReportDate)
				.Where(x => x.ManagementOfChangeStatus == ManagementOfChangeStatus.Approved);

			var response = await query
					.OrderByDescending(moc => moc.CreatedDateTime)
					.ToQueryOperationResponseAsync<ManagementOfChangeRecord>();

			var records = response.ToList();
			Count = (int)response.Count;
			Items = new Collection<DailyOperatingChange>(records
				.Select(x => new DailyOperatingChange
				{
					Id = x.Id,
					Date = ReportDate.Value,
					TagName = x.Identification.SCETagNumber,
					RaisedDate = x.CreatedDateTime,
					ExpiredDate = x.Identification.ExpiryDate,
					ReferenceNumber = x.RecordNumber,
					Remark = x.TitleOfChange
				})
				.ToArray());
			var itemCountBefore = Items.Count;
			await ItemsChanged.InvokeAsync(Items);
			await OnItemsChanged.InvokeAsync(Items);
			var itemCountAfter = Items.Count;
			Count = itemCountAfter - itemCountBefore + Count;
			_items = Items;
			isLoading = false;
        }

		private IGenericService<ManagementOfChangeRecord> GetGenericMOCService(IServiceScope serviceScope)
		{
			return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<ManagementOfChangeRecord, IManagementOfChangeUnitOfWork>>();
		}
	}
}
