using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Reports;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.PEMonthlyReports
{
	public partial class PEMonthlyReportView
	{
		private readonly bool isEditing;
		private bool isLoading = true;
		private bool isUserHavePermission = true;
		private DateTime date;

		[Parameter] public PEReport Data { get; set; }

		[Inject] private DialogService DialogService { get; set; }

		[Inject] private IServiceProvider ServiceProvider { get; set; }

		[Inject] private AffraNotificationService AffraNotificationService { get; set; }
		[Inject] private NavigationManager NavManager { get; set; }

		[Inject] private IUserService UserService { get; set; }

		[Inject] private IReportService ReportService { get; set; }

		[Inject] private IJSRuntime JSRuntime { get; set; }

		private CommonFilter CommonFilter { get; set; }


		protected override async Task OnInitializedAsync()
		{
			this.date = this.Data.Date.LocalDateTime;
            CommonFilter = new CommonFilter(NavManager)
			{
				Date = Data.Date.UtcDateTime
			};

			isLoading = false;
			isUserHavePermission = await UserService.CheckHasPermissionAsync(null, new Permission { Name = nameof(FeaturePermission.CombinedDailyReport), HasReadPermissoin = true, HasWritePermission = true });
			await base.OnInitializedAsync();
		}

		private IGenericService<PEReport> GetGenericService(IServiceScope serviceScope)
		{
			return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<PEReport, ICentralizedDatabaseSystemUnitOfWork>>();
		}

		private async Task ApproveAsync()
		{
			try
			{
				using var scope = ServiceProvider.CreateScope();
				var service = GetGenericService(scope);
				Data.Status = PEReportStatus.Approved;
				await service.UpdateAsync(Data, Data.Id);
				AffraNotificationService.NotifySuccess("Report approved.");
				DialogService.Close();
			}
			catch (Exception ex)
			{
				AffraNotificationService.NotifyException(ex);
			}
		}

		private async Task RejectAsync()
		{
			try
			{
				using var scope = ServiceProvider.CreateScope();
				var service = GetGenericService(scope);
				Data.Status = PEReportStatus.Rejected;
				await service.UpdateAsync(Data, Data.Id);
				AffraNotificationService.NotifySuccess("Report rejected.");
				DialogService.Close();
			}
			catch (Exception ex)
			{
				AffraNotificationService.NotifyException(ex);
			}
		}

		private async Task DownloadAsync()
		{
			isLoading = true;
			try
			{
				var streamResult = await ReportService.GeneratePEReportAsync(Data);
				if (streamResult != null)
				{
					using var streamRef = new DotNetStreamReference(streamResult);
					await JSRuntime.InvokeVoidAsync("downloadFileFromStream", $"PEReport{Data.Date.ToLocalTime():yyyy MMMM}.xlsx", streamRef);
				}
			}
			catch (Exception ex)
			{
				AffraNotificationService.NotifyException(ex);
			}
			isLoading = false;
		}
	}
}
