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
        public string ItemID { get; set; }
        public string Name { get; set; }

        public Item(string itemID, string name)
        {
            ItemID = itemID ?? throw new ArgumentNullException(nameof(itemID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
