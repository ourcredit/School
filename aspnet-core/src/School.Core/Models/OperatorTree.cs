using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace School.Models
{
    /// <summary>
    /// 机构树
    /// </summary>
    [Table("s_operator_tree")]
  public  class OperatorTree:CreationAuditedEntity
    {
        /// <summary>
        /// 节点名
        /// </summary>
        [Required,MaxLength(120)]
        public string TreeName { get; set; }
        /// <summary>
        /// 节点编号
        /// </summary>
        public string TreeCode { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int TreeLength { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        public  virtual int? ParentId { get; set; }
        /// <summary>
        /// 上级对象
        /// </summary>
        public  virtual  OperatorTree Parent { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        [ForeignKey("ParentId")]
        public  virtual ICollection<OperatorTree> Children { get; set; }
        /// <summary>
        /// 下届设备
        /// </summary>
        [ForeignKey("OperatorId")]
        public virtual ICollection<OperatorDevice> OperatorDevices { get; set; }
    }

    /// <summary>
    /// temp modal
    /// </summary>
    public class dsc_drp_shop
    {
        public string shop_name { get; set; }
        public int user_id { get; set; }
        public string email { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string ec_salt { get; set; }
    }
}
