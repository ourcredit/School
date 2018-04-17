using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace School.Models
{
    /// <summary>
    /// 设备表
    /// </summary>
    [Table("s_device")]
  public  class Device:CreationAuditedEntity,ISoftDelete
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
        public bool State { get; set; }
        /// <summary>
        /// 点位对象
        /// </summary>
        public virtual Point Point { get; set; }
        [ForeignKey("DeviceId")]
        public  virtual ICollection<DeviceGood> DeviceGoods { get; set; }

        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// 设备下的商品绑定
    /// </summary>
    [Table("s_device_goods")]
    public class DeviceGood : CreationAuditedEntity<int>,ISoftDelete
    {
        public DeviceGood() { }
        public DeviceGood(int deviceId, int gId, string name, int price)
        {
            DeviceId = deviceId;
            GoodsId = gId;
            GoodsName = name;
            Price = price;
        }
        public int DeviceId { get; set; }
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

        public bool IsDeleted { get; set; }
    }
}
