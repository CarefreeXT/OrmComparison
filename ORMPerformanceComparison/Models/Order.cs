using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mego = Caredev.Mego.DataAnnotations;
using ef = System.ComponentModel.DataAnnotations;
using ef1 = System.ComponentModel.DataAnnotations.Schema;
using SqlSugar;

namespace ORMPerformanceComparison.Models
{
    [mego.Table("Orders")]
    [ef1.Table("Orders")]
    [SugarTable("Orders")]
    public class Order
    {
        [mego.Key]
        [ef.Key, ef1.DatabaseGenerated(ef1.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public int State { get; set; }

        [SugarColumn(IsIgnore = true)]
        [mego.ForeignKey("CustomerId", "Id")]
        public virtual Customer Customer { get; set; }

        [SugarColumn(IsIgnore = true)]
        [mego.InverseProperty("OrderId", "Id")]
        public virtual ICollection<OrderDetail> Details { get; set; }
    }
}
