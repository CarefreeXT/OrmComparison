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
    [mego.Table("OrderDetails")]
    [ef1.Table("OrderDetails")]
    [SugarTable("OrderDetails")]
    public class OrderDetail
    {

        [mego.Key, mego.Identity]
        [ef.Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ef1.Column(nameof(ProductId))]
        public int? ProductId { get; set; }

        public Guid Key { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        [SugarColumn(IsIgnore = true)]
        [mego.ForeignKey("OrderId", "Id")]
        public virtual Order Order { get; set; }

        [SugarColumn(IsIgnore = true)]
        [mego.ForeignKey("ProductId", "Id")]
        public virtual Product Product { get; set; }
    }
}
