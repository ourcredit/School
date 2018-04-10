using System;
using Abp.Application.Services.Dto;
using School.Models;

namespace School.Points.Dtos
{
    public class PointListDto : CreationAuditedEntityDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string PointName { get; set; }
        public string PointAddress { get; set; }
        public string PointDescription { get; set; }
        public string Longitude { get; set; }
        public string Latitide { get; set; }
    }
}