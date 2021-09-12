using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database.BusinessEntity
{
    [Table("KG_Now")]
    public class Now
    {
        /// <summary>
        /// 容器ID
        /// </summary>
        [Required]
        public int ContainerID { get; set; }
        /// <summary>
        /// 位置ID
        /// </summary>
        [Required]
        public int LocationID { get; set; }
        /// <summary>
        /// 进入时间
        /// </summary>
        [Required]
        public DateTime EnterTime { get; set; }

        public Location Location { get; set; }
        
        public NowMes NowMes { get; set; }
    }
}
