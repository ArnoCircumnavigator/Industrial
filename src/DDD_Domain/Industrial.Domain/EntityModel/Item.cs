using System;

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
