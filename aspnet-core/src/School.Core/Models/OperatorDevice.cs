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
   public class OperatorDevice:CreationAuditedEntity<int>
    {
        public OperatorDevice() { }

        public OperatorDevice(int orgId, int deviceId)
        {
            OperatorId = orgId;
            DeviceId = deviceId;
        }
        /// <summary>
        /// 设备id
        /// </summary>
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }
        public int OperatorId { get; set; }
        [ForeignKey("OperatorDeviceId")]
        public virtual  ICollection<OperatorDeviceGoods> DeviceGoodses { get; set; }
    }
    /// <summary>
    /// 设备下的商品绑定
    /// </summary>
    [Table("OperatorDeviceGoods")]
    public class OperatorDeviceGoods : CreationAuditedEntity<Guid>
    {
        public int OperatorDeviceId { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public int Price { get; set; }
    }
}
