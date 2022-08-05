using System.Collections.Concurrent;

namespace JXNippon.CentralizedDatabaseSystem.Domain.DataSources
{
    public class GlobalDataSource : IGlobalDataSource
    {
        private int _unreadCount;
        private object _unreadCountLock = new object();
        public ConcurrentBag<Exception> Exceptions { get; } = new ConcurrentBag<Exception>();

        public int UnreadCount
        {
            get
            {
                return this._unreadCount;
            }
            set
            {
                lock (_unreadCountLock)
                {
                    this._unreadCount = value;
                }
            }
        }

        public bool IsDevelopment { get; } = true;

        public void AddException(Exception exception)
        { 
            this.Exceptions.Add(exception);
            if (this.Exceptions.Count > 10)
            {
                _ = this.Exceptions.TryTake(out Exception firstException);
            }
        }
    }
}
