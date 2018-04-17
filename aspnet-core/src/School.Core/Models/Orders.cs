using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace School.Models
{
    /// <summary>
    /// 订单表
    /// </summary>
    [Table("Gx_vm_order")]
   public class Orders:Entity
    {
        public string order_id { get; set; }
        public string transaction_id { get; set; }
        /// <summary>
        /// 售货机id
        /// </summary>
        public string vmid { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int goods_id { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string goods_name { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public float pay_price { get; set; }
        //支付渠道
        public int pay_Channel { get; set; }
        /// <summary>
        /// 商户编号
        /// </summary>
        public int merchant_id { get; set; }
        /// <summary>
        /// 商户名
        /// </summary>
        public string merchant_name { get; set; }
        /// <summary>
        /// 取货码
        /// </summary>
        public string pickup_code { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created_time { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? pay_time { get; set; }
        /// <summary>
        /// 出货时间
        /// </summary>
        public DateTime? delivery_time { get; set; }
        /// <summary>
        /// 订单状态 0等待支付、1支付成功、2正在出货、出货成功、出货失败、退款
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 支付账号
        /// </summary>
        public string pay_account { get; set; }
        public string notify_url { get; set; }
    }
    /// <summary>
    /// 商品表
    /// </summary>
    public class dsc_Goods
    {
        public int goods_id { get; set; }
        public int cat_id { get; set; }
        public string goods_img { get; set; }
        public int user_cat { get; set; }
        public string goods_sn { get; set; }
        public string bar_code { get; set; }
        public string goods_name { get; set; }
        public string goods_name_style { get; set; }
        public int brand_id { get; set; }
        public string provider_name { get; set; }
        public int goods_number { get; set; }
        public int user_id { get; set; }
        public string cat_name { get; set; }

        public bool IsSeal { get; set; }
        public int Price { get; set; }
    }

    /// <summary>
    /// 货道信息表
    /// </summary>
    [Table("Gx_vm_channel")]
    public class Channel : Entity
    {
        public string Machine_Code { get; set; }
        public int Goods_Id { get; set; }
        public float Quantity { get; set; }
        public float QuantityLine { get; set; }
        public int Site { get; set; }
        public int State { get; set; }
        public bool Isdelete { get; set; }
        public DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 设备展示位
    /// </summary>
    [Table("Gx_vm_Show")]
    public class Show : Entity
    {
        public string Machine_Code { get; set; }
        public int Goods_id { get; set; }
        public int Site { get; set; }
        public bool Isdelete { get; set; }
        public DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 中间表
    /// </summary>
    [Table("Gx_vm_Show2Channel")]
    public class ChannelShow : Entity
    {
        public string Machine_Code { get; set; }
        public int ShowSite { get; set; }
        public string ChannelSite { get; set; }
        public bool Isdelete { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
