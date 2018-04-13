using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using School.Devices.Dtos;

namespace School.Others.Dtos
{
    /// <summary>
    /// 更新货道
    /// </summary>
    public class CreateOrUpdateChannelInput
    {
        [Required]
        public List<ChannelDto> Gx_vm_channel { get; set; }

    }
    /// <summary>
    /// 货道dtro
    /// </summary>
    public class ChannelDto
    {
        public string MachineCode { get; set; }
        public int Goods_id { get; set; }
        public float Quantity { get; set; }
        public float QuantityLine { get; set; }
        public int Site { get; set; }
        public int State { get; set; }
    }
    /// <summary>
    /// 更新展示柜
    /// </summary>
    public class CreateOrUpdateShowInput
    {
        [Required]
        public List<ShowDto> Gx_vm_Show { get; set; }
    }

    public class ShowDto
    {
        public string MachineCode { get; set; }
        public int Goods_id { get; set; }
        public int Site { get; set; }
    }

    public class CreateOrUpdateShowChannelInput
    {
        [Required]
        public List<ShowChannelDto> Gx_vm_Show2Channel { get; set; }
    }
    /// <summary>
    /// 展示柜dto
    /// </summary>
    public class ShowChannelDto
    {
        public string MachineCode { get; set; }
        public int ShowSite { get; set; }
        public string ChannelSite { get; set; }
    }
}