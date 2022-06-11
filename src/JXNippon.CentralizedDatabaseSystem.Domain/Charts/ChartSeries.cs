using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Charts
{
    public class ChartSeries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int Sequence { get; set; }

        [StringLength(50)]

        public string CategoryProperty { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string ValueProperty { get; set; }

        public bool Smooth { get; set; }

        [StringLength(10)]
        public string LineType { get; set; }

        [StringLength(10)]
        public string MarkerType { get; set; }


        [StringLength(200)]
        public string GroupProperty { get; set; }

    }
}
