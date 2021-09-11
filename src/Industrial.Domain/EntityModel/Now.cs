using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Domain.EntityModel
{
    public class Now
    {
        /// <summary>
        /// 容器ID
        /// </summary>
        public int ContainerID { get; set; }
        /// <summary>
        /// 位置ID
        /// </summary>
        public int LocationID { get; set; }
        /// <summary>
        /// 进入时间
        /// </summary>
        public DateTime EnterTime { get; set; }

        public Location Location { get; set; }
        
        public NowMes NowMes { get; set; }
    }
}
