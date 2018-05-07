using System;
using Abp.Runtime.Validation;
using School.Dto;

namespace School.Others.Dtos
{
    /// <summary>
    /// 获取设备商品input
    /// </summary>
    public class GetDeviceGoodsInput : PagedInputDto
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }
    }
      /// <summary>
    /// 获取设备商品input
    /// </summary>
    public class GetProductsInput 
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }
    }
    /// <summary>
    /// 获取订单列表 参数
    /// </summary>

    public class GetOrderInput: PagedInputDto
    {
        public string TreeCode { get; set; }
        public string OrderNum { get; set; }
        public string DeviceNum { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
    /// <summary>
    /// 获取商品input
    /// </summary>
    public class GetGoodsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// leixin
        /// </summary>
        public string Sn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Cate { get; set; }
        /// <summary>
        /// 是否售卖
        /// </summary>
        public bool? IsSeal { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

    }
}
