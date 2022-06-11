using Affra.Core.Domain.UnitOfWorks;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Views
{
    public interface IViewUnitOfWork : IUnitOfWork
    {
        IGenericRepository<View> ViewRepository { get; }

        IGenericRepository<Row> RowRepository { get; }

        IGenericRepository<Column> ColumnRepository { get; }
    }
}