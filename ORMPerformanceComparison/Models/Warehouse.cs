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
    [mego.Table("Warehouses")]
    [ef1.Table("Warehouses")]
    [SugarTable("Warehouses")]
    public class Warehouse
    {
        [mego.Key, mego.Column(nameof(Id), Order = 1)]
        [ef.Key, ef1.Column(nameof(Id), Order = 1)]
        [ef1.DatabaseGenerated(ef1.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [mego.Key, mego.Column(nameof(Number), Order = 2)]
        [ef.Key, ef1.Column(nameof(Number), Order = 2)]
        [ef1.DatabaseGenerated(ef1.DatabaseGeneratedOption.None)]
        public int Number { get; set; }

        public string Name { get; set; }
       
        public string Address { get; set; }
    }
}
