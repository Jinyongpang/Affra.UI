namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? ToLocalDateTime(this DateTimeOffset? dateTimeOffset)
        {
            return dateTimeOffset is null
                ? null
                : dateTimeOffset.Value.LocalDateTime;
        }

        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.LocalDateTime;
        }

        public static DateTimeOffset? ToUniversalTime(this DateTime? dateTime)
        {
            return dateTime is null
                ? null
                : dateTime.Value.ToUniversalTime();
        }
        public static string ToDateString(this DateTime? dateTime)
        {
            return dateTime is null
                ? null
                : dateTime.Value.ToDateString();
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("d");
        }
        public static string ToDateString(this DateTimeOffset? dateTime)
        {
            return dateTime is null
                ? null
                : dateTime.Value.ToDateString();
        }

        public static string ToDateString(this DateTimeOffset dateTime)
        {
            return dateTime.ToString("d");
        }
    }
}
