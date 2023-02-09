using System;
using System.Collections.Generic;

namespace Industrial.Infra.Database.Models
{
    public partial class KgLocation
    {
        public KgLocation()
        {
            KgNows = new HashSet<KgNow>();
        }

        public int LocationId { get; set; }
        public LocStatus Status { get; set; }
        public LoadStatus LoadStatus { get; set; }

        public virtual ICollection<KgNow> KgNows { get; set; }
    }
    public enum LocStatus
    {
        normal,
        onlyIn,
        onlyOut,
        disable
    }
    public enum LoadStatus
    {
        idle,
        load
    }
}
