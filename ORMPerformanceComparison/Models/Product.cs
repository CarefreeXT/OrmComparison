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
    [mego.Table("Products")]
    [ef1.Table("Products")]
    [SugarTable("Products")]
    public class Product
    {
        [mego.Key, mego.Identity]
        [ef.Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Category { get; set; }

        public bool IsValid { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
