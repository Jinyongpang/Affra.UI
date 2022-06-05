using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace ViewODataService.Affra.Service.View.Domain.Views
{
    public partial class LineChart
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
    }
}
