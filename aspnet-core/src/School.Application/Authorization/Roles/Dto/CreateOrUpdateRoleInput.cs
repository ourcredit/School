using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Authorization.Roles.Dto
{/// <summary>
/// 
/// </summary>
    public class CreateOrUpdateRoleInput
    {/// <summary>
    /// 
    /// </summary>
        [Required]
        public RoleEditDto Role { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}