using System.Collections.ObjectModel;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using Microsoft.OData.Extensions.Client;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Dashboard
    {
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        private View view = new View();

        protected override Task OnInitializedAsync()
        {
            return GetViewAsync();
        }

        protected async Task GetViewAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            DataServiceQuery<View> query = (DataServiceQuery<View>)this.GetGenericService<View>(serviceScope)
                .Get();
            query = query.Expand(view => view.Rows);
            Task<IEnumerable<View>> getView = GetViewAndRowsAsync();
            Task<IEnumerable<LineChart>> getLineChart = GetLineChartsAsync();

            await Task.WhenAll(getView, getLineChart);

            view = (await getView).FirstOrDefault();

            if (view != null)
            {
                foreach (LineChart? lineChart in await getLineChart)
                {
                    Row? row = view.Rows
                        .Where(row => row.Id == lineChart.RowId)
                        .FirstOrDefault();
                    row.Columns = new Collection<ColumnBase>();
                    row.Columns.Add(lineChart);
                }
            }
        }

        private Task<IEnumerable<View>> GetViewAndRowsAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            DataServiceQuery<View> query = (DataServiceQuery<View>)this.GetGenericService<View>(serviceScope)
                .Get();
            query = query.Expand(view => view.Rows);
            return query
                .Where(d => d.Name == nameof(Dashboard))
                .ExecuteAsync<View>();
        }

        private Task<IEnumerable<LineChart>> GetLineChartsAsync()
        {
            using var serviceScope = ServiceProvider.CreateScope();
            DataServiceQuery<LineChart> query = (DataServiceQuery<LineChart>)this.GetGenericService<LineChart>(serviceScope)
                .Get();
            query = query.Expand(linechart => linechart.ChartSeries);
            return query
                .Where(d => d.ViewName == nameof(Dashboard))
                .OrderBy(d => d.Sequence)
                .ExecuteAsync<LineChart>();
        }

        private IGenericService<T> GetGenericService<T>(IServiceScope serviceScope)
            where T : class
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<T, IViewUnitOfWork>>();
        }
    }
}
