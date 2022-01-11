using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database.BusinessEntity
{
    [Table("KG_Item")]
    public class Item
    {
        [Required]
        public int ItemID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
