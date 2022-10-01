using System.Collections.Concurrent;
using JXNippon.CentralizedDatabaseSystem.Domain.Filters;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Domain.DataSources
{
    public class GlobalDataSource : IGlobalDataSource
    {
        private int _unreadCount;
        private User _user;
        private object _usertLock = new object();
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
                lock (this._unreadCountLock)
                {
                    this._unreadCount = value;
                }
            }
        }

        public bool IsDevelopment { get; } = true;
        public User User
        {
            get
            {
                return this._user;
            }
            set
            {
                lock (this._usertLock)
                {
                    this._user = value;
                }
            }
        }

        public IDateFilterComponent GlobalDateFilter { get; set; }

        public object LoginDisplay { get; set; }

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
