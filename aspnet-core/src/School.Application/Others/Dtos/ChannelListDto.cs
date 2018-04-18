using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace School.Others.Dtos
{
    /// <summary>
    /// 
    /// </summary>
   public class ChannelListDto:EntityDto
    {
        public string Machine_Code { get; set; }
        public int Goods_Id { get; set; }
        public string Goods_Name { get; set; }
        public float Quantity { get; set; }
        public float QuantityLine { get; set; }
        public int Site { get; set; }
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class ShowListDto : EntityDto
    {
        public string Machine_Code { get; set; }
        public int Goods_id { get; set; }
        public string Goods_Name { get; set; }
        public int Site { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class CheckPickCodeResult
    {
        public bool IsTrue { get; set; }
        public int ProductId { get; set; }
    }
}
