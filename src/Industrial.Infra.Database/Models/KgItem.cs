using System;
using System.Collections.Generic;

namespace Industrial.Infra.Database.Models
{
    public partial class KgItem
    {
        public KgItem()
        {
            KgNowmes = new HashSet<KgNowme>();
        }

        public int ItemId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<KgNowme> KgNowmes { get; set; }
    }
}
