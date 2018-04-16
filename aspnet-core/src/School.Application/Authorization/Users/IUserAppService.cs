using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Authorization.Users.Dto;
using School.Dto;

namespace School.Authorization.Users
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input);
        /// <summary>
        /// 导出jui
        /// </summary>
        /// <returns></returns>
        Task<FileDto> GetUsersToExcel();
        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);
        /// <summary>
        /// 重置权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ResetUserSpecificPermissions(EntityDto<long> input);
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateUserPermissions(UpdateUserPermissionsInput input);
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteUser(EntityDto<long> input);
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UnlockUser(EntityDto<long> input);
    }
}