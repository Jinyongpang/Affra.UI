using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Grids
{
    public class GridColumn
    {
        public string Property { get; set; }

        public string Title { get; set; }

        public string FormatString { get; set; }

        public string Type { get; set; }

        public bool Filterable { get; set; }

        public bool Sortable { get; set; } = true;

        public bool Frozen { get; set; }

        public string Width { get; set; }

        [IgnoreClientProperty]
        [JsonIgnore]
        public Type ActualType
        {
            get
            {
                return string.IsNullOrEmpty(this.Type)
                    ? null
                    : System.Type.GetType(ViewHelper.GetTypeMapping()[this.Type]);
            }
            set
            {
                this.Type = value.Name;
            }
        }

        public ICollection<ConditionalStyling> ConditionalStylings { get; set; }

    }
}
