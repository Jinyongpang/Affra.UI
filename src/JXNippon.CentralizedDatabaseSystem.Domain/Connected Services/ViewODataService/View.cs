using Microsoft.OData.Client;

namespace ViewODataService.Affra.Service.View.Domain.Views
{
    public partial class View
    {
        [IgnoreClientProperty]
        public ICollection<Row> RowsStartFromSecond 
        { 
            get 
            {   List<Row> list = new List<Row>();
                for (int i = 1; i < Rows.Count; i++)
                { 
                    list.Add(this.Rows[i]);
                }
                return list;
            } 
        }

        [IgnoreClientProperty]
        public ICollection<Row> RowsFirst
        {
            get
            {
                List<Row> list = new List<Row>();
                for (int i = 0; i < Rows.Count && i < 1; i++)
                {
                    list.Add(this.Rows[i]);
                }
                return list;
            }
        }
    }
}
