using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Domain.EntityModel
{
    public class NowMes
    {
        public int ContainerID { get; set; }
        public int Qty { get; set; }
        public int ItemID { get; set; }


        public Now Now { get; set; }
        public Item Item { get; set; }
    }
}
