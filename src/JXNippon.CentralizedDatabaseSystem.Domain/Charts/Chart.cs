using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Charts
{
    public class Chart : ColumnComponent
    {
        public string Type { get; set; }

        public string FormatString { get; set; }

        public int StepInMinutes { get; set; }

        public decimal? ValueAxisStep { get; set; }

        public string AxisTitle { get; set; }

        public ICollection<ChartSeries> ChartSeries { get; set; }

        [IgnoreClientProperty]
        [JsonIgnore]
        public Type ActualType
        {
            get
            {
                return System.Type.GetType(ViewHelper.GetTypeMapping()[this.Type]);
            }
            set
            {
                this.Type = value.Name;
            }
        }
    }
}
