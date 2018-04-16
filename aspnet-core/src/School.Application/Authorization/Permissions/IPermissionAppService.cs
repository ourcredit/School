using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Authorization.Permissions.Dto;

namespace School.Authorization.Permissions
{/// <summary>
/// 
/// </summary>
    public interface IPermissionAppService : IApplicationService
    {/// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
