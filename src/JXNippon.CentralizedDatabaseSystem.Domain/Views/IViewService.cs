using Microsoft.Extensions.DependencyInjection;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Views
{
    public interface IViewService
    {
        Task<IEnumerable<View>> GetViewsAndRowsAsync(string name = null);
        Task<View> GetViewAsync(string name);
        Task GetViewDetailAsync(View view);
        Task<IEnumerable<Column>> GetColumnsAsync(View view);
        IDictionary<string, string> GetTypeMapping();
        object GetPropValue(object src, string propName);
        dynamic GetGenericService(IServiceScope serviceScope, string typeInString);
        Task<IEnumerable<View>> GetPageViewsAsync(string page);
        IDictionary<string, string> GetExtraTypeMapping();
    }
}