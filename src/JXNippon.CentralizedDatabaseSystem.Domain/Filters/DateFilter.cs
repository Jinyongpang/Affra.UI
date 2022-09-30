using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Filters
{
    public class DateFilter : ColumnComponent
    {
        public DateFilterType DateFilterType { get; set; }

        public string Id { get; set; }

        [IgnoreClientProperty]
        [JsonIgnore]
        public string DateFilterTypeString
        {
            get
            {
                return DateFilterType.ToString();
            }
            set
            {
                DateFilterType = (DateFilterType)Enum.Parse(typeof(DateFilterType), value);
            }
        }
    }
}
