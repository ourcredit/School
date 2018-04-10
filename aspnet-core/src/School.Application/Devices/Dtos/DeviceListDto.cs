using System;
using Abp.Application.Services.Dto;
using School.Models;

namespace School.Devices.Dtos
{
    public class DeviceListDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string DeviceNum { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public int PointId { get; set; }
        public string PointPointName { get; set; }
        public string PointPointAddress { get; set; }
        public string PointPointDescription { get; set; }
        public string PointLongitude { get; set; }
        public string PointLatitide { get; set; }
    }
}