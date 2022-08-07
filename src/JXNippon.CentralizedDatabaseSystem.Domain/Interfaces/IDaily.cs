using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Interfaces
{
    public interface IDaily
    {
        DateTimeOffset Date { get; set; }

        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
