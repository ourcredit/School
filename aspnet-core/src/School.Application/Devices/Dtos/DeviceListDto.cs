using System;
using Abp.Application.Services.Dto;
using School.Models;

namespace School.Devices.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceListDto:CreationAuditedEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string DeviceNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 工控编号
        /// </summary>
        public string ControlNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PointId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool State { get; set; }
    }
}