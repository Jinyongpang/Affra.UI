using System.Collections.Concurrent;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Domain.DataSources
{
    public interface IGlobalDataSource
    {
        ConcurrentBag<Exception> Exceptions { get; }
        int UnreadCount { get; set; }
        bool IsDevelopment { get; }

        void AddException(Exception exception);

        User User { get; set; }

        object LoginDisplay { get; set; }
    }
}