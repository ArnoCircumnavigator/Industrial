using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database.BusinessEntity
{
    [Table("KG_Job")]
    public class Job
    {
        [Required]
        public long JobUniqueID { get; set; }
        [Required]
        public int JobId { get; set; }
    }
}
