using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace School.Models
{
    /// <summary>
    /// 点位表
    /// </summary>
    [Table("s_point")]
   public class Point:CreationAuditedEntity
    {
        /// <summary>
        /// 点位名称
        /// </summary>
        [Required,MaxLength(120)]
        public string PointName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string PointAddress { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string PointDescription { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string Latitide { get; set; }
    }
}
