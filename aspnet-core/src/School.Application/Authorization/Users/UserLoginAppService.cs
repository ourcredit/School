using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using School.Authorization.Users.Dto;

namespace School.Authorization.Users
{
    /// <summary>
    /// 
    /// </summary>
    [AbpAuthorize]
    public class UserLoginAppService : SchoolAppServiceBase, IUserLoginAppService
    {
        private readonly IRepository<UserLoginAttempt, long> _userLoginAttemptRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLoginAttemptRepository"></param>
        public UserLoginAppService(IRepository<UserLoginAttempt, long> userLoginAttemptRepository)
        {
            _userLoginAttemptRepository = userLoginAttemptRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts()
        {
            var userId = AbpSession.GetUserId();

            var loginAttempts = await _userLoginAttemptRepository.GetAll()
                .Where(la => la.UserId == userId)
                .OrderByDescending(la => la.CreationTime)
                .Take(10)
                .ToListAsync();

            return new ListResultDto<UserLoginAttemptDto>(ObjectMapper.Map<List<UserLoginAttemptDto>>(loginAttempts));
        }
    }
}