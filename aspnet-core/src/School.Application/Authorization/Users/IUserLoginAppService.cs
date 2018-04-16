using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Authorization.Users.Dto;

namespace School.Authorization.Users
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserLoginAppService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
