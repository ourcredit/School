using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Authorization.Users.Dto
{ /// <summary>
  /// 
  /// </summary>
    public class CreateOrUpdateUserInput
    { /// <summary>
      /// 
      /// </summary>
        [Required]
        public UserEditDto User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string[] AssignedRoleNames { get; set; }
    }
}