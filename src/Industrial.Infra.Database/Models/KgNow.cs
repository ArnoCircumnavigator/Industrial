using System;
using System.Collections.Generic;

namespace Industrial.Infra.Database.Models
{
    public partial class KgNow
    {
        public int ContainerId { get; set; }
        public int LocationId { get; set; }
        public DateTime EnterTime { get; set; }

        public virtual KgLocation Location { get; set; }
        public virtual KgNowmes KgNowmes { get; set; }
    }
}
