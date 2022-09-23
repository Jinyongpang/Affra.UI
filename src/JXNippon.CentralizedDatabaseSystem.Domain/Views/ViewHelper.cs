using System.Reflection;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Views
{
    public static class ViewHelper
    {
        private static IDictionary<string, string> TypeMappings;
        private static IDictionary<string, string> ExtraTypeMappings;
        public static IDictionary<string, string> GetExtraTypeMapping()
        {
            Assembly assembly = typeof(View).Assembly;
            return ExtraTypeMappings ??= assembly.GetTypes()
                .DistinctBy(t => t.Name)
                .Where(t => t.GetInterface(nameof(IExtras)) != null)
                .OrderBy(t => t.Name)
                .ToDictionary(x => x.Name, x => x.AssemblyQualifiedName);
        }

        public static IDictionary<string, string> GetTypeMapping()
        {
            Assembly assembly = typeof(View).Assembly;
            return TypeMappings ??= assembly.GetTypes()
                .DistinctBy(t => t.Name)
                .Where(t => t.GetInterface(nameof(IDaily)) != null)
                .OrderBy(t => t.Name)
                .ToDictionary(x => x.Name, x => x.AssemblyQualifiedName);
        }

        public static Type GetActualType(string type)
        {
            return string.IsNullOrEmpty(type)
                ? null
                : Type.GetType(ViewHelper.GetTypeMapping()[type]);
        }

        public static IDictionary<string, object> ToDictionaryObject(this object value, string prefix = "")
        {
            return value is null
                ? null
                : value.GetType().GetProperties().ToDictionary(property => $"{prefix}{property.Name}", property => property.GetValue(value));
        }

        public static IEnumerable<IDictionary<string, object>> ToDictionaryObjectList(this IEnumerable<object> items)
        {
            if (items is null)
            {
                return null;
            }
            List<IDictionary<string, object>> keyValuePairs = new List<IDictionary<string, object>>();

            foreach (object item in items)
            {
                keyValuePairs.Add(item.ToDictionaryObject());
            }

            return keyValuePairs;
        }

        public static string ToStringFormat(this object value, string format = null)
        {
            if (value is null)
            {
                return null;
            }
            else if (value is int integer)
            {
                return integer.ToString(format);
            }
            else if (value is decimal dec)
            {
                return dec.ToString(format);
            }
            else if (value is DateTime dateTime)
            {
                return dateTime.ToString(format);
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.ToString(format);
            }
            else if (value is double doub)
            {
                return doub.ToString(format);
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
