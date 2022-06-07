using System.Collections.ObjectModel;
using Microsoft.OData.Client;
using Microsoft.OData.Extensions.Client;
using ViewODataService.Affra.Service.View.Domain.Charts;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Views
{
    public class ViewService : IViewService
    {
        private readonly IViewUnitOfWork viewUnitOfWork;
        public ViewService(IViewUnitOfWork viewUnitOfWork)
        {
            this.viewUnitOfWork = viewUnitOfWork;
        }

        public IDictionary<string, string> GetTypeMapping()
        {
            return ViewHelper.GetTypeMapping();
        }

        public async Task<View> GetViewAsync(string name)
        {
            DataServiceQuery<View> query = (DataServiceQuery<View>)viewUnitOfWork.ViewRepository.Get();
            query = query.Expand(view => view.Rows);
            Task<IEnumerable<View>> getView = GetViewsAndRowsAsync(name);
            Task<IEnumerable<Chart>> getChart = GetChartsAsync(name);

            await Task.WhenAll(getView, getChart);

            View view = (await getView).FirstOrDefault();

            ConstructView(view, await getChart);

            return view;
        }

        public async Task GetViewDetailAsync(View view)
        {
            if (view != null)
            {
                var columns = await GetColumnsAsync(view);
                ConstructView(view, columns);
            }
        }

        public async Task<IEnumerable<ColumnBase>> GetColumnsAsync(View view)
        {
            List<ColumnBase> columnBases = new List<ColumnBase>();
            if (view != null
                && !string.IsNullOrEmpty(view.Name))
            {
                IEnumerable<Chart> charts = await GetChartsAsync(view.Name);
                columnBases.AddRange(charts.Cast<ColumnBase>());
            }
            return columnBases;
        }

        public Task<IEnumerable<View>> GetViewsAndRowsAsync(string name = null)
        {
            DataServiceQuery<View> query = (DataServiceQuery<View>)viewUnitOfWork.ViewRepository.Get();
            query = query.Expand(view => view.Rows);

            if (!string.IsNullOrEmpty(name))
            {
                query = (DataServiceQuery<View>)query
                    .Where(d => d.Name == name);
            }
            return query.ExecuteAsync<View>();
        }

        private Task<IEnumerable<Chart>> GetChartsAsync(string name = null)
        {
            DataServiceQuery<Chart> query = (DataServiceQuery<Chart>)viewUnitOfWork.ChartRepository.Get();
            query = query.Expand(chart => chart.ChartSeries);
            query = query.Expand(chart => chart.Row);

            if (!string.IsNullOrEmpty(name))
            {
                query = (DataServiceQuery<Chart>)query
                    .Where(d => d.ViewName == name);
            }
            return query
                .OrderBy(d => d.Sequence)
                .ExecuteAsync<Chart>();
        }


        private void ConstructView(View view, IEnumerable<ColumnBase> columnBases)
        {
            if (view != null)
            {
                foreach (Row row in view.Rows)
                {
                    row.Columns = new Collection<ColumnBase>();
                }
                foreach (var column in columnBases)
                {
                    Row? row = view.Rows
                        .Where(row => row.Id == column.RowId)
                        .FirstOrDefault();
                    row.Columns.Add(column);
                    column.Row = row;
                    column.View = view;
                }
            }
        }

    }
}
