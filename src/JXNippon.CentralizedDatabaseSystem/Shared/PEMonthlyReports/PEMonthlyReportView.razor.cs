using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.PEReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
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
        [Inject] private IGlobalDataSource GlobalDataSource { get; set; }

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
                if (this.Data.Status == PEReportStatus.Approved)
                {
                    AffraNotificationService.NotifyInfo("The report has already been approved.");
                }
                else
                {
                    using var scope = ServiceProvider.CreateScope();
                    var service = GetGenericService(scope);
                    var referenceId = await this.ReportService.GeneratePEReportAsync(Data);
                    Data.Status = PEReportStatus.Approved;
                    Data.Revision++;
                    Data.User = this.GlobalDataSource.User.Email;
                    Data.LastApproval = new()
                    {
                        ApprovedDateTime = DateTime.UtcNow,
                        ReportReferenceId = referenceId,
                        Revision = Data.Revision,
                        ApprovedBy = this.GlobalDataSource.User.Name,
                        FileName = $"PE_{Data.DateUI:ydd}_{Data.DateUI:MMMM}_Rpt_Rev{Data.Revision}.xlsx",
                    };
                    Data.Approvals.Add(Data.LastApproval);
                    await service.UpdateAsync(Data, Data.Id);
                    AffraNotificationService.NotifySuccess("Report approved.");
                    DialogService.Close();
                }
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
                Data.User = this.GlobalDataSource.User.Email;
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
                if (Data.LastApproval is null)
                {
                    throw new InvalidOperationException("Report never approved before.");
                }
                var streamResult = await ReportService.DownloadReportAsync(Data.LastApproval.ReportReferenceId);
                if (streamResult != null)
                {
                    using var streamRef = new DotNetStreamReference(streamResult);
                    await JSRuntime.InvokeVoidAsync("downloadFileFromStream", Data.LastApproval.FileName, streamRef);
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
