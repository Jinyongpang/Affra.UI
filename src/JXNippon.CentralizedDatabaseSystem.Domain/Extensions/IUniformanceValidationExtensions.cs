using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    public static class IUniformanceValidationExtensions
	{
        public static IUniformanceValidation AsIUniformanceValidation(this object entity)
        {
            return (IUniformanceValidation)entity;
        }
    }
}
