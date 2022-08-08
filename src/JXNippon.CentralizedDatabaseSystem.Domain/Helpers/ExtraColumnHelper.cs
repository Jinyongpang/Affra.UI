using System.Text.Json;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Helpers
{
    public static class ExtraColumnHelper
    {
        public static IDictionary<string, object>? ToExtrasObject(this string extras)
        {
            return string.IsNullOrEmpty(extras)
                ? new Dictionary<string, object>()
                : JsonSerializer.Deserialize<IDictionary<string, object>>(extras);
        }
    }
}
