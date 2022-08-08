using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Domain.TemplateManagements
{
    public interface IExtraColumnService
    {
        Type GetType(string type);
        string GetStringValue(CustomColumn customColumn, IDictionary<string, object>? extraObject);

        IQueryable<CustomColumn> GetCustomColumns(string tableName);

        IDictionary<string, object> GetExtraObject(IEnumerable<CustomColumn> customColumns, IDictionary<string, object>? extraObject);
    }
}