using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Authorization.Permissions.Dto;

namespace School.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
