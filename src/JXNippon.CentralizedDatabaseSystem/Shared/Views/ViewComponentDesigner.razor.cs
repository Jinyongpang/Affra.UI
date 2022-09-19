using System.Text.Json;
using System.Text.Json.Serialization;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Announcements;
using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using JXNippon.CentralizedDatabaseSystem.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class ViewComponentDesigner : IAsyncDisposable
    {
        private bool _disposed = false;
        private IEnumerable<View> views = new List<View>();
        private RadzenDropDown<View> radzenDropDown;
        private View view = new View();
        private ViewComponent viewComponent;
        private ColumnDataGrid columnDataGrid;
        private RowDataGrid rowDataGrid;
        private ICollection<ICollection<string>> colorsGroups = new List<ICollection<string>>();

        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        public async Task ReloadAsync(View value = null)
        {
            value ??= this.view;
            await this.GetViewDetailAsync(value);   
            this.view = value;
            this.ReloadColorGroup();
            StateHasChanged();
            await viewComponent.ReloadAsync();
        }

        private void ReloadColorGroup()
        {
            colorsGroups = new List<ICollection<string>>();
            foreach (var row in view.Rows)
                foreach (var col in row.Columns)
                {
                    colorsGroups.Add(Constants.Constant.GetRandomColors());
                }
        }

        protected override async Task OnInitializedAsync()
        {
            views = (await GetViewsAsync()).ToList();
            this.view = views.FirstOrDefault();
            await this.GetViewDetailAsync();
            this.ReloadColorGroup();

        }

        protected async Task<IEnumerable<View>> GetViewsAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            return await viewService.GetViewsAndRowsAsync();

        }

        private async Task GetViewDetailAsync(View value = null)
        {
            view = value ?? this.view;
            using var serviceScope = ServiceProvider.CreateScope();
            IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
            await viewService.GetViewDetailAsync(view);
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            if (view != null
                && !string.IsNullOrEmpty(view.Name))
            {
                using var serviceScope = ServiceProvider.CreateScope();
                IViewService viewService = serviceScope.ServiceProvider.GetService<IViewService>();
                views = (await GetViewsAsync()).ToList();
                radzenDropDown.SelectedItem = views
                    .Where(x => x.Name == view?.Name)
                    .FirstOrDefault();
                view = radzenDropDown.SelectedItem as View;
                await viewService.GetViewDetailAsync(view);
                await viewComponent.ReloadAsync();
            }
        }

        private async Task OnChangeAsync(object value)
        {
            view = value as View;
            await this.ReloadAsync();
        }

        private async Task AddRowAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var genericService = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<Row, IViewUnitOfWork>>();
            var seq = view.Rows.Count > 0
                ? view.Rows.Max(row => row.Sequence) + 1
                : 0;
            var row = new Row() { ViewName = view.Name, Sequence = seq };
            await genericService.InsertAsync(row);
            AffraNotificationService.NotifyItemCreated();
            view.Rows.Add(row);
            StateHasChanged();
        }

        private async Task AddViewAsync()
        {
            View newView = new View();
            dynamic? response = await DialogService.OpenAsync<ViewDialog>("Add View",
                           new Dictionary<string, object>() { { "Item", newView } },
                           Constant.DialogOptions);

            if (response == true)
            {
                try
                {
                    await this.AddViewAsync(newView);
                    this.views = (await GetViewsAsync()).ToList();
                    this.view = this.views.LastOrDefault();
                    StateHasChanged();
                    AffraNotificationService.NotifyItemCreated();
                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
                finally
                {
                }
            }
        }

        private async Task EditViewAsync()
        {
            dynamic? response = await DialogService.OpenAsync<ViewDialog>("Edit View",
                           new Dictionary<string, object>() { { "Item", this.view } },
                           Constant.DialogOptions);

            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService<View>(serviceScope);
                    await service.UpdateAsync(this.view, this.view.Id);
                    StateHasChanged();
                    AffraNotificationService.NotifyItemUpdated();
                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
                finally
                {
                }
            }
        }

        private async Task DuplicateViewAsync()
        {
            View newView = new View();
            dynamic? response = await DialogService.OpenAsync<ViewDialog>("Duplicate View",
                           new Dictionary<string, object>() { { "Item", newView } },
                           Constant.DialogOptions);

            if (response == true)
            {
                try
                {
                    await this.AddViewAsync(this.view, newView.Name);
                    this.views = (await GetViewsAsync()).ToList();
                    this.view = this.views.LastOrDefault();
                    await this.ReloadAsync(view);
                    AffraNotificationService.NotifyItemCreated();

                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
                finally
                {
                }
            }
        }

        private async Task DeleteViewAsync()
        {
            if (await DialogService.OpenAsync<GenericConfirmationDialog>($"Deleting view: {view.Name}",
                    new Dictionary<string, object>() { },
                    new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true }))
            {
                using var serviceScope = ServiceProvider.CreateScope();
                var genericService = serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<View, IViewUnitOfWork>>();
                await genericService.DeleteAsync(this.view);
                this.views = (await GetViewsAsync()).ToList();
                this.view = new View();
                StateHasChanged();
                AffraNotificationService.NotifyItemDeleted();
            }
        }

        private IGenericService<T> GetGenericService<T>(IServiceScope serviceScope)
            where T : class
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<T, IViewUnitOfWork>>();
        }

        private async Task ShowDialogAsync(Column data, string title)
        {
            dynamic? response = null;
            if (data.ComponentType == nameof(Chart))
            {
                response = await DialogService.OpenAsync<ChartDialog>(title,
                   new Dictionary<string, object>() { { "Column", data }, { "View", view } },
                   Constant.FullScreenDialogOptions);
            }
            else if (data.ComponentType == nameof(Grid))
            {
                response = await DialogService.OpenAsync<GridDialog>(title,
                   new Dictionary<string, object>() { { "Column", data }, { "View", view } },
                   Constant.FullScreenDialogOptions);
            }
            else if (data.ComponentType == nameof(AnnouncementCard))
            {
                response = await DialogService.OpenAsync<AnnouncementCardDialog>(title,
                   new Dictionary<string, object>() { { "Column", data }, { "View", view } },
                   Constant.FullScreenDialogOptions);
            }


            if (response == true)
            {
                try
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var service = this.GetGenericService<Column>(serviceScope);

                    if (data.Id > 0)
                    {
                        await service.UpdateAsync(data, data.Id);
                        AffraNotificationService.NotifyItemUpdated();
                    }
                    else
                    {
                        var row = this.view.Rows.LastOrDefault();
                        if (row is null)
                        {
                            await this.AddRowAsync();
                            row = this.view.Rows.LastOrDefault();
                        }
                        data.RowId = row.Id;
                        data.Sequence = row.Columns.Count;
                        await service.InsertAsync(data);
                        AffraNotificationService.NotifyItemCreated();
                    }

                }
                catch (Exception ex)
                {
                    AffraNotificationService.NotifyException(ex);
                }
                finally
                {
                    await this.ReloadAsync();
                }
            }
        }

        private async Task ExportAsync()
        {
            var content = JsonSerializer.SerializeToUtf8Bytes(view, options: new() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
            var fileName = $"{view.Name}.json";

            using var streamRef = new DotNetStreamReference(stream: new MemoryStream(content));

            await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

        private async Task ImportAsync(InputFileChangeEventArgs e)
        {
            try
            {
                var view = await JsonSerializer.DeserializeAsync<View>(e.File.OpenReadStream());

                if (await DialogService.OpenAsync<GenericConfirmationDialog>($"Importing view: {view.Name}",
                    new Dictionary<string, object>() { },
                    new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true }))
                {
                    using var serviceScope = ServiceProvider.CreateScope();
                    var unitOfWork = serviceScope.ServiceProvider.GetRequiredService<IViewUnitOfWork>();

                    View existingView = (await unitOfWork.ViewRepository.Get()
                        .Where(predicate: x => x.Name == view.Name)
                        .ToQueryOperationResponseAsync<View>())
                        .FirstOrDefault();

                    if (existingView is not null)
                    {
                        if (await DialogService.OpenAsync<GenericConfirmationDialog>($"Replacing existing view? View: {view.Name}",
                        new Dictionary<string, object>() { },
                        new DialogOptions() { Style = Constant.DialogStyle, Resizable = true, Draggable = true }) == false)
                        {
                            return;
                        }
                        else
                        {
                            unitOfWork.ViewRepository.Delete(existingView);
                            await unitOfWork.SaveAsync();
                        }
                    }

                    await this.AddViewAsync(view);
                    await this.ReloadAsync(view);
                    AffraNotificationService.NotifyItemCreated();
                }
            }
            catch (Exception ex)
            {
                AffraNotificationService.NotifyException(ex);
            }
        }

        private async Task AddViewAsync(View view, string viewName = null)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var unitOfWork = serviceScope.ServiceProvider.GetRequiredService<IViewUnitOfWork>();
            view.Id = 0;
            view.Name = viewName ?? view.Name;
            unitOfWork.ViewRepository.Insert(view);
            await unitOfWork.SaveAsync();
            foreach (var row in view.Rows)
            {
                row.Id = 0;
                row.ViewName = view.Name;
                unitOfWork.RowRepository.Insert(row);
                await unitOfWork.SaveAsync();
                foreach (var column in row.Columns)
                {
                    column.Id = 0;
                    column.ViewName = view.Name;
                    column.RowId = row.Id;
                    unitOfWork.ColumnRepository.Insert(column);
                    await unitOfWork.SaveAsync();
                }
            }

            views = (await GetViewsAsync()).ToList(); 
        }

        public async ValueTask DisposeAsync()
        {
            if (!this._disposed)
            { 
                await this.viewComponent.DisposeAsync();           
                this._disposed = true;
            }
        }
    }
}
