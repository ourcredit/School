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
}
