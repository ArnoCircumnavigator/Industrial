using System;
using System.Collections.Generic;

namespace Industrial.Infra.Database.Models
{
    public partial class KgJob
    {
        public int JobId { get; set; }
        public long JobUniqueId { get; set; }
    }
}
