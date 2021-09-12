using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database.BusinessEntity
{
    [Table("KG_Location")]
    public class Location
    {
        /// <summary>
        /// 位置ID
        /// </summary>
        [Required]
        public int LocationID { get; set; }
        [Required]
        public LocStatus Status { get; set; } = LocStatus.Normal;
        [Required]
        public LocLoadStatus LoadStatus { get; set; } = LocLoadStatus.idle;
    }
    /// <summary>
    /// 位置状态
    /// </summary>
    public enum LocStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 禁入
        /// </summary>
        DisableIn,
        /// <summary>
        /// 禁出
        /// </summary>
        DisableOut,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable
    }
    /// <summary>
    /// 位置载货状态
    /// </summary>
    public enum LocLoadStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        idle,
        /// <summary>
        /// 忙碌
        /// </summary>
        busy
    }
}
