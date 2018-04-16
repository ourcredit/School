using System.Collections.Generic;
using School.Authorization.Permissions.Dto;

namespace School.Authorization.Roles.Dto
{/// <summary>
/// 
/// </summary>
    public class GetRoleForEditOutput
    {/// <summary>
    /// 
    /// </summary>
        public RoleEditDto Role { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }
/// <summary>
/// 
/// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}