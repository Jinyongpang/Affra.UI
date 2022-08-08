using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JXNippon.CentralizedDatabaseSystem.Domain.Helpers;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Interfaces
{
    public interface IExtras
    {
        [IgnoreClientProperty]
        IDictionary<string, object>? ExtraDictionary
        {
            get
            {
                return this.Extras.ToExtrasObject();
            }
        }

        string Extras { get; set; }
    }
}
