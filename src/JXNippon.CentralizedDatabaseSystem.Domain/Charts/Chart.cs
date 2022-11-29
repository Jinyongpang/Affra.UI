using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Charts
{
    public class Chart : ColumnComponent
    {
        public string Type { get; set; }

        public string FormatString { get; set; }

        public int StepInMinutes { get; set; }

        public decimal? ValueAxisStep { get; set; }

        public string AxisTitle { get; set; }

        public string DateFilterId { get; set; }

        public string Style { get; set; }

        public bool ShowLegend { get; set; } = true;

        public LegendPosition LegendPosition { get; set; } = LegendPosition.Bottom;

        public ICollection<ChartSeries> ChartSeries { get; set; }

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

        [IgnoreClientProperty]
        [JsonIgnore]
        public string LegendPositionString
        {
            get
            {
                return LegendPosition.ToString();
            }
            set
            {
                this.LegendPosition = (LegendPosition)Enum.Parse(typeof(LegendPosition), value, true);
            }
        }
    }
}
