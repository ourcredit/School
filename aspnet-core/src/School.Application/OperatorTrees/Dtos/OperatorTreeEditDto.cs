using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Models;

namespace School.OperatorTrees.Dtos
{
    public class OperatorTreeEditDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public int? Id { get; set; }
        /// <summary>
        /// 节点名
        /// </summary>
        [Required, MaxLength(120)]
        public string TreeName { get; set; }
        public int? ParentId { get; set; }
    }
}