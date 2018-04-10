using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using School.Models;

namespace School.OperatorTrees.Dtos
{
    public class CreateOrUpdateOperatorTreeInput
{
////BCC/ BEGIN CUSTOM CODE SECTION
////ECC/ END CUSTOM CODE SECTION
        [Required]
        public OperatorTreeEditDto OperatorTree { get; set; }

}
}