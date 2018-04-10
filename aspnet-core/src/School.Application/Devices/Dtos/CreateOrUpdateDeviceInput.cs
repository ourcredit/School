using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using School.Models;

namespace School.Devices.Dtos
{
    public class CreateOrUpdateDeviceInput
{
////BCC/ BEGIN CUSTOM CODE SECTION
////ECC/ END CUSTOM CODE SECTION
        [Required]
        public DeviceEditDto Device { get; set; }

}
}