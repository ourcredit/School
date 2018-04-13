using Abp.Runtime.Validation;
using School.Dto;

namespace School.Others.Dtos
{
    public class GetDeviceGoodsInput : PagedInputDto
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }
    }
    public class GetGoodsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public string Filter { get; set; }
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
