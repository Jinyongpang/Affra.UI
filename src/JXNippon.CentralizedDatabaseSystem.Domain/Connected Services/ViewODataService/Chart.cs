using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace ViewODataService.Affra.Service.View.Domain.Charts
{
    public partial class Chart
    {
        [IgnoreClientProperty]
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
