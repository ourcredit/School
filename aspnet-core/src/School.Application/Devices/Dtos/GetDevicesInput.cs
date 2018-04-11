using System.Collections.Generic;
using Abp.Runtime.Validation;
using School.Dto;
using School.Models;

namespace School.Devices.Dtos
{
    public class GetDevicesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public string Filter { get; set; }

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
}
