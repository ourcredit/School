using System.ComponentModel.DataAnnotations;
using School.Models;

namespace School.Devices.Dtos
{
    public class DeviceEditDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public int? Id { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        [Required, MaxLength(128)]
        public string DeviceNum { get; set; }
        /// <summary>
        /// 工控编号
        /// </summary>
        public string ControlNum { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public int PointId { get; set; }
    }
}