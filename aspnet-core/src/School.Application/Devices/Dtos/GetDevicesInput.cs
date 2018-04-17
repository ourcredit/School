using System.Collections.Generic;
using Abp.Runtime.Validation;
using School.Dto;
using School.Models;

namespace School.Devices.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDevicesInput : PagedAndSortedInputDto, IShouldNormalize
    {
       /// <summary>
       /// 名称
       /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Cate { get; set; }

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
    /// <summary>
    /// 根据机构id 获取设备信息
    /// </summary>
    public class GetOrgsDevicesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public int OrgId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Num { get; set; }
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

    public class BindDevicesInput
    {
        public int OrgId { get; set; }
        public List<int> Devices { get; set; }
    }

    public class BindGoodsInput
    {
        public int DeviceId { get; set; }
        public List<GoodsDto> Goods { get; set; }
    }

    public class GoodsDto
    {
        public int GoodId { get; set; }
        public string GoodName { get; set; }
        public int Price { get; set; }  
    }
}
