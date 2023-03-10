using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Grids
{
    public class Grid : ColumnComponent
    {
        public string Type { get; set; }
        public bool Split3Months { get; set; }
        public string Style { get; set; } = "height: 45vh;";

        public string DateFilterId { get; set; }

        public ICollection<GridColumn> GridColumns { get; set; }

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
    }
}
