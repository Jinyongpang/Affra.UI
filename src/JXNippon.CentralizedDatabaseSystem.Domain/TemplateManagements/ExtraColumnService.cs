using System.Text.Json;
using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Domain.TemplateManagements
{
    public class ExtraColumnService : IExtraColumnService
    {
        private readonly IUnitGenericService<CustomColumn, IViewUnitOfWork> unitGenericService;

        public ExtraColumnService(IUnitGenericService<CustomColumn, IViewUnitOfWork> unitGenericService)
        {
            this.unitGenericService = unitGenericService;
        }
        private IDictionary<string, Type> TypeMappings = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            ["string"] = typeof(string),
            ["bool"] = typeof(bool),
            ["integer"] = typeof(int),
            ["decimal"] = typeof(decimal),
            ["date"] = typeof(DateTime),
            ["datetime"] = typeof(DateTime),
        };

        public Type GetType(string type)
        {
            return this.TypeMappings[type];
        }

        public string GetStringValue(CustomColumn customColumn, IDictionary<string, object>? extraObject)
        {
            if (extraObject is null)
            {
                return string.Empty;
            }

            var result = extraObject.TryGetValue(customColumn.PropertyName, out var value);
            if (!result || value is null)
            {
                return string.Empty;
            }
            switch (customColumn.Type)
            {
                case "date":
                    var date = (value as JsonElement?).GetValueOrDefault().GetDateTime();
                    return date.ToLocalTime().ToString("d");
                case "datetime":
                    var dateTime = (value as JsonElement?).GetValueOrDefault().GetDateTime();
                    return dateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                default:
                    return value?.ToString() ?? string.Empty;
            }
        }

        public IDictionary<string, object> GetExtraObject(IEnumerable<CustomColumn> customColumns, IDictionary<string, object>? extraObject)
        {
            IDictionary<string, object> extrasObject = new Dictionary<string, object>();
            foreach (var customColumn in customColumns)
            {
                JsonElement? jsonElement;
                object? value = null;
                _ = extraObject?.TryGetValue(customColumn.PropertyName, out value);
                jsonElement = (JsonElement?)value;
                if (jsonElement is null)
                {
                    extrasObject[customColumn.PropertyName] = null;
                    continue;
                }

                if (jsonElement is null)
                {
                    extrasObject[customColumn.PropertyName] = null;
                }
                JsonElement jsonValue = jsonElement.GetValueOrDefault();
                
                switch (customColumn.Type)
                {       
                    case "string":
                        if (jsonValue.ValueKind == JsonValueKind.String)
                        {
                            extrasObject[customColumn.PropertyName] = jsonValue.ToString();
                        }
                        else
                        {
                            extrasObject[customColumn.PropertyName] = null;
                        }
                        break;
                    case "date":
                        if (jsonValue.ValueKind == JsonValueKind.String
                            && jsonValue.TryGetDateTime(out var date))
                        {
                            extrasObject[customColumn.PropertyName] = date;
                        }
                        else
                        {
                            extrasObject[customColumn.PropertyName] = null;
                        }
                        break;
                    case "datetime":
                        if (jsonValue.ValueKind == JsonValueKind.String
                            && jsonValue.TryGetDateTime(out var dateTime))
                        {
                            extrasObject[customColumn.PropertyName] = dateTime;
                        }
                        else
                        {
                            extrasObject[customColumn.PropertyName] = null;
                        }
                        break;
                    case "integer":
                        if (jsonValue.ValueKind == JsonValueKind.Number)
                        {
                            extrasObject[customColumn.PropertyName] = jsonValue.GetInt32();
                        }
                        else
                        {
                            extrasObject[customColumn.PropertyName] = null;
                        }
                        break;
                    case "decimal":
                        if (jsonValue.ValueKind == JsonValueKind.Number)
                        {
                            extrasObject[customColumn.PropertyName] = jsonValue.GetDecimal();
                        }
                        else
                        {
                            extrasObject[customColumn.PropertyName] = null;
                        }
                        break;
                    case "bool":
                        extrasObject[customColumn.PropertyName] = jsonValue.ValueKind == JsonValueKind.True;
                        break;
                    default:
                        extrasObject[customColumn.PropertyName] = null;
                        break;
                }                
            }

            return extrasObject;
        }

        public IQueryable<CustomColumn> GetCustomColumns(string tableName)
        {
            return unitGenericService
                .Get()
                .Where(item => item.Table == tableName);
        }
    }
}
