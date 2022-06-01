using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Interfaces
{
    public interface IDaily
    {
        DateTimeOffset Date { get; set; }

        [IgnoreClientProperty]
        DateTime DateUI { get; set; }
    }
}
