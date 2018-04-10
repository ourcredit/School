using System.ComponentModel.DataAnnotations;
using School.Models;

namespace School.Points.Dtos
{
    public class PointEditDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public int? Id { get; set; }
        /// <summary>
        /// 点位名称
        /// </summary>
        [Required, MaxLength(120)]
        public string PointName { get; set; }
        public string PointAddress { get; set; }
        public string PointDescription { get; set; }
        public string Longitude { get; set; }
        public string Latitide { get; set; }
    }
}