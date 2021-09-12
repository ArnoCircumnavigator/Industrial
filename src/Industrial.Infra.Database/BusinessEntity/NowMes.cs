using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database.BusinessEntity
{
    [Table("KG_NowMes")]
    public class NowMes
    {
        [Required]
        public int ContainerID { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public int ItemID { get; set; }


        public Now Now { get; set; }
        public Item Item { get; set; }
    }
}
