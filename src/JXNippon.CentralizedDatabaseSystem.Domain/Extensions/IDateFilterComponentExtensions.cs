using JXNippon.CentralizedDatabaseSystem.Domain.Filters;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    public static class IDateFilterComponentExtensions
    {
        public static IDateFilterComponent AsIDateFilterComponent(this object entity)
        {
            return entity as IDateFilterComponent;
        }
    }
}
