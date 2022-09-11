using System.Text.Json.Serialization;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Grids
{
    public class ConditionalStyling
    {
        public ConditionalStylingOperator Operator { get; set; }

        public string Value { get; set; }

        public string BackgroundColor { get; set; }

        public string FontColor { get; set; }

        public string Style { get; set; }


        [IgnoreClientProperty]
        [JsonIgnore]
        public string OperatorString
        {
            get
            {
                return Operator.ToString();
            }
            set
            {
                this.Operator = (ConditionalStylingOperator)Enum.Parse(typeof(ConditionalStylingOperator), value, true);
            }
        }
    }
}
