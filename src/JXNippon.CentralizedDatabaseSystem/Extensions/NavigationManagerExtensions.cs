using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace JXNippon.CentralizedDatabaseSystem.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static T? GetQueryString<T>(this NavigationManager navManager, string key)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
            {
                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    return (T)(object)valueAsInt;
                }

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)valueFromQueryString.ToString();
                }

                if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
                {
                    return (T)(object)valueAsDecimal;
                }

                if (typeof(T) == typeof(DateTime?) && DateTime.TryParse(valueFromQueryString, out DateTime valueAsDateTime))
                {
                    return (T)(object)valueAsDateTime;
                }

            }
            return default;
        }
    }
}
