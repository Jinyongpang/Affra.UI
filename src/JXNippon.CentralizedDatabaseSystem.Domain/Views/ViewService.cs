using System.Collections.ObjectModel;
using Microsoft.OData.Client;
using Microsoft.OData.Extensions.Client;
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
            Task<IEnumerable<LineChart>> getLineChart = GetLineChartsAsync(name);

            await Task.WhenAll(getView, getLineChart);

            View view = (await getView).FirstOrDefault();

            ConstructView(view, await getLineChart);

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
                IEnumerable<LineChart> lineCharts = await GetLineChartsAsync(view.Name);
                columnBases.AddRange(lineCharts.Cast<ColumnBase>());
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

        private Task<IEnumerable<LineChart>> GetLineChartsAsync(string name = null)
        {
            DataServiceQuery<LineChart> query = (DataServiceQuery<LineChart>)viewUnitOfWork.LineChartRepository.Get();
            query = query.Expand(linechart => linechart.ChartSeries);
            query = query.Expand(linechart => linechart.Row);

            if (!string.IsNullOrEmpty(name))
            {
                query = (DataServiceQuery<LineChart>)query
                    .Where(d => d.ViewName == name);
            }
            return query
                .OrderBy(d => d.Sequence)
                .ExecuteAsync<LineChart>();
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
