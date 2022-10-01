using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Statistics
{
    public class Statistic : ColumnComponent
    {
        public string Type { get; set; }
        public string Property { get; set; }

        public int Precision { get; set; }

        public string Suffix { get; set; }

        public bool ComparePrevious { get; set; }

        public string ColorGreater { get; set; }

        public string ColorLesser { get; set; }

        public string ColorEqual { get; set; }

        public StatisticsCompareType CompareType { get; set; }

        public string DateFilterId { get; set; }

        [IgnoreClientProperty]
        [JsonIgnore]
        public string CompareTypeString
        {
            get
            {
                return CompareType.ToString();
            }
            set
            {
                CompareType = (StatisticsCompareType)Enum.Parse(typeof(StatisticsCompareType), value);
            }
        }

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
