using Microsoft.OData.Client;

namespace ViewODataService.Affra.Service.View.Domain.Views
{
    public partial class ColumnBase
    {
        [IgnoreClientProperty]
        public int RowSequence 
        { 
            get 
            {
                return this.Row?.Sequence ?? -1; 
            } 
            set 
            { 
                this.Row = this.View?.Rows.Where(row => row.Sequence == value).FirstOrDefault();
                this.RowId = this.Row?.Id;
            } 
        }
    }
}
