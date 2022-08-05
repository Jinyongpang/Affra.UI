using System.Collections.Concurrent;

namespace JXNippon.CentralizedDatabaseSystem.Domain.DataSources
{
    public interface IGlobalDataSource
    {
        ConcurrentBag<Exception> Exceptions { get; }
        int UnreadCount { get; set; }
        bool IsDevelopment { get; }

        void AddException(Exception exception);
    }
}