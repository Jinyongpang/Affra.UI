using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    public static class IDailyExtensions
    {
        public static IDaily AsIDaily(this object dailyObject)
        {
            return (IDaily)dailyObject;
        }
    }
}
