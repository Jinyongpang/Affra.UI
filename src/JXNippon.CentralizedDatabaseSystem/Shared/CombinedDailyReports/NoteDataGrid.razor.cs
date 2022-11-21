using Affra.Core.Domain.Services;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.CombinedDailyReports;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class NoteDataGrid<TItem>
        where TItem : class, IEntity
    {
        private int count = 0;
        private IQueryable<SectionNote> data;
        [Parameter] public Collection<SectionNote> Data { get; set; }
        [Parameter] public TItem CombinedDailyReport { get; set; }
        [Parameter] public EventCallback<Collection<SectionNote>> DataChanged { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ContextMenuService ContextMenuService { get; set; }

        protected override void OnInitialized()
        {
            count = Data.Count;
            data = Data.AsQueryable();
            base.OnInitialized();
        }

        private Task OnDataChangedAsync(Collection<SectionNote> data)
        {
            return DataChanged.InvokeAsync(data);
        }

        private async Task ShowDialogAsync(SectionNote data, int menuAction, string title)
        {
            ContextMenuService.Close();
            dynamic? response;
            if (menuAction == 2)
            {
                response = await DialogService.OpenAsync<GenericConfirmationDialog>(title,
                           new Dictionary<string, object>() { },
                           new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true });

                if (response == true)
                {
                    Data.Remove(data);
                    using var serviceScope = ServiceProvider.CreateScope();
                    var cdrService = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
                    await cdrService.UpdateAsync(CombinedDailyReport, CombinedDailyReport.Id);
                    await OnDataChangedAsync(Data);
                    AffraNotificationService.NotifyItemDeleted();
                    this.data = Data.AsQueryable();
                    count = Data.Count;
                    StateHasChanged();
                }
            }
            else
            {
                response = await DialogService.OpenAsync<NoteDialog>(title,
                           new Dictionary<string, object>() { { "Data", data }, { "MenuAction", menuAction } },
                           Constant.DialogOptions);

                if (response == true)
                {
                    try
                    {
                        if (menuAction == 0)
                        {
                            Data.Add(data);
                        }
                        using var serviceScope = ServiceProvider.CreateScope();
                        var cdrService = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
                        await cdrService.UpdateAsync(CombinedDailyReport, CombinedDailyReport.Id);
                        await OnDataChangedAsync(Data);
                    }
                    catch (Exception ex)
                    {
                        AffraNotificationService.NotifyException(ex);
                    }

                    AffraNotificationService.NotifyItemUpdated();
                    this.data = Data.AsQueryable();
                    count = Data.Count;
                    StateHasChanged();
                }
            }
        }
    }
}
