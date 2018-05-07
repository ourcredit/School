using System;
using System.Collections.Generic;
using System.Text;

namespace School.Others.Dtos
{
    /// <summary>
    /// 商品列表dto
    /// </summary>
   public class ProductListDto
    {
        public float price { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string img_url { get; set; }
    }

    public class OrderListDto
    {
        public string order_id { get; set; }
   
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
        /// 取货码
        /// </summary>
        public string pickup_code { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created_time { get; set; }
        /// <summary>
        /// 货柜号
        /// </summary>

        public int site { get; set; }
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
     
     
    }
}
