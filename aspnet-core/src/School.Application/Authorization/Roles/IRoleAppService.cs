using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Authorization.Roles.Dto;

namespace School.Authorization.Roles
{
    /// <summary>
    /// Application service that is used by 'role management' page.
    /// </summary>
    public interface IRoleAppService : IApplicationService
    { /// <summary>
        /// 
        /// </summary>
        Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input);
 /// <summary>
        /// 
        /// </summary>
        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input);
 /// <summary>
        /// 
        /// </summary>
        Task CreateOrUpdateRole(CreateOrUpdateRoleInput input);
 /// <summary>
        /// 
        /// </summary>
        Task DeleteRole(EntityDto input);
    }
}