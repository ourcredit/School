using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace School.Models
{
    /// <summary>
    /// 设备表
    /// </summary>
    [Table("s_device")]
  public  class Device:CreationAuditedEntity
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Required,MaxLength(128)]
        public string DeviceNum { get; set; }
        /// <summary>
        /// 设备名
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 点位id
        /// </summary>
        public virtual int PointId { get; set; }
        /// <summary>
        /// 点位对象
        /// </summary>
        public virtual Point Point { get; set; }

    }
}
