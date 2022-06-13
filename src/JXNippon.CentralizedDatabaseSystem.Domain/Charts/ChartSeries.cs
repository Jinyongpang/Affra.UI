using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Charts
{
    public class ChartSeries
    {

        public int Sequence { get; set; }

        public string CategoryProperty { get; set; }

        public string Title { get; set; }

        public string ValueProperty { get; set; }

        public bool Smooth { get; set; }

        public string LineType { get; set; }

        public string MarkerType { get; set; }

        public string GroupProperty { get; set; }

        public ExecutionType ExecutionType { get; set; }

        public string ExecutionTypeString 
        {
            get
            { 
                return ExecutionType.ToString();
            }
            set
            { 
                ExecutionType = (ExecutionType)Enum.Parse(typeof(ExecutionType), value);
            }
        }

        public ChartType ChartType { get; set; }

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
