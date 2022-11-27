using System.Collections.ObjectModel;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Uniformances;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Interfaces
{
    public interface IUniformanceValidation
	{
        public Collection<UniformanceResult> UniformanceResults { get; set; }
    }
}
