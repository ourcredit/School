using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using School.Models;

namespace School.Points.Dtos
{
    public class CreateOrUpdatePointInput
{
////BCC/ BEGIN CUSTOM CODE SECTION
////ECC/ END CUSTOM CODE SECTION
        [Required]
        public PointEditDto Point { get; set; }

}
}