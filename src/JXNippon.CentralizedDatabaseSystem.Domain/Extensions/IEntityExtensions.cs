using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    public static class IEntityExtensions
    {
        public static IEntity AsIEntity(this object entity)
        {
            return (IEntity)entity;
        }
    }
}
