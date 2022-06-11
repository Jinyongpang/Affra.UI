using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Charts
{
    public class Chart
    {
        [StringLength(500)]
        public string Type { get; set; }

        [StringLength(100)]
        public string FormatString { get; set; }

        public int StepInMinutes { get; set; }

        [StringLength(50)]
        public string AxisTitle { get; set; }

        public ChartType ChartType { get; set; }

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

        [IgnoreClientProperty]
        [JsonIgnore]
        public string ChartTypeString
        {
            get
            {
                return ChartType.ToString();
            }
            set
            {
                this.ChartType = (ChartType)Enum.Parse(typeof(ChartType), value, true);
            }
        }
    }
}
