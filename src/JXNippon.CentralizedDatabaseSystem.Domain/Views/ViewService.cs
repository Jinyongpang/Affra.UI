using System.Collections.ObjectModel;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using Microsoft.Extensions.DependencyInjection;
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

        public object GetPropValue(object src, string propName)
        {
            return src?.GetType()?.GetProperty(propName)?.GetValue(src, null) ?? null;
        }

        public dynamic GetGenericService(IServiceScope serviceScope, string typeInString)
        {
            var type = typeof(IUnitGenericService<,>).MakeGenericType(Type.GetType(typeInString), typeof(ICentralizedDatabaseSystemUnitOfWork));
            return serviceScope.ServiceProvider.GetRequiredService(type);
        }

        public async Task<View> GetViewAsync(string name)
        {
            DataServiceQuery<View> query = (DataServiceQuery<View>)viewUnitOfWork.ViewRepository.Get();
            query = query.Expand(view => view.Rows);
            Task<IEnumerable<View>> getView = GetViewsAndRowsAsync(name);
            Task<IEnumerable<Column>> getColumns = GetColumnsAsync(name);

            await Task.WhenAll(getView, getColumns);

            View view = (await getView).FirstOrDefault();

            ConstructView(view, await getColumns);

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

        public async Task<IEnumerable<Column>> GetColumnsAsync(View view)
        {
            List<Column> columnBases = new List<Column>();
            if (view != null
                && !string.IsNullOrEmpty(view.Name))
            {
                IEnumerable<Column> columns = await GetColumnsAsync(view.Name);
                columnBases.AddRange(columns);
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

        private Task<IEnumerable<Column>> GetColumnsAsync(string name = null)
        {
            DataServiceQuery<Column> query = (DataServiceQuery<Column>)viewUnitOfWork.ColumnRepository.Get();
            query = query.Expand(column => column.Row);
            if (!string.IsNullOrEmpty(name))
            {
                query = (DataServiceQuery<Column>)query
                    .Where(d => d.ViewName == name);
            }
            return query
                .OrderBy(d => d.Sequence)
                .ExecuteAsync<Column>();
        }


        private void ConstructView(View view, IEnumerable<Column> columnBases)
        {
            if (view != null)
            {
                foreach (Row row in view.Rows)
                {
                    row.Columns = new Collection<Column>();
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
