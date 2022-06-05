using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Views
{
    public interface IViewService
    {
        Task<IEnumerable<View>> GetViewsAndRowsAsync(string name = null);
        Task<View> GetViewAsync(string name);
        Task GetViewDetailAsync(View view);
        Task<IEnumerable<ColumnBase>> GetColumnsAsync(View view);
        IDictionary<string, string> GetTypeMapping();
    }
}