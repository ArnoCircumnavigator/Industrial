using System;
using System.Collections.Generic;

namespace Industrial.Infra.Database.Models
{
    public partial class KgNowmes
    {
        public int ContainerId { get; set; }
        public int Qty { get; set; }
        public int ItemId { get; set; }

        public virtual KgNow Container { get; set; }
        public virtual KgItem Item { get; set; }
    }
}
