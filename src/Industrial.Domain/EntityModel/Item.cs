using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Domain.EntityModel
{
    public class Item
    {
        public int ItemID { get; private set; }
        public string Name { get; private set; }
    }
}
