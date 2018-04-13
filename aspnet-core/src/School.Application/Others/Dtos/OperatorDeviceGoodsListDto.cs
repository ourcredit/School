using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace School.Others.Dtos
{
   public class DeviceGoodsListDto:EntityDto
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
