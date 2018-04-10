using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using School.Models;

namespace School.OperatorTrees.Dtos
{
    public class OperatorTreeListDto:CreationAuditedEntityDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string TreeName { get; set; }
        public string TreeCode { get; set; }
        public int TreeLength { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
    }
}