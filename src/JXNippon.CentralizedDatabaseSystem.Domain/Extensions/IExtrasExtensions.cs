using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    public static class IExtrasExtensions
    {
        public static IExtras AsIExtras(this object extraObject)
        {
            return (IExtras)extraObject;
        }
    }
}
