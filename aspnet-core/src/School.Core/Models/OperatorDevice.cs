using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace School.Models
{
    /// <summary>
    /// 设备和机构树关系表
    /// </summary>
    [Table("s_operator_device")]
   public class OperatorDevice:CreationAuditedEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }
        public int OperatorId { get; set; }
        /// <summary>
        /// 是否售卖
        /// </summary>
        public bool IsSeal { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public int Price { get; set; }
    }
}
