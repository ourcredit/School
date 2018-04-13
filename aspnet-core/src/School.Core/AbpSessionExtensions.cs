using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using School.Authorization.Users;

namespace School
{
    /// <summary>
    /// 
    /// </summary>
    public static class AbpSessionExtensions
    {

        /// <summary>
        /// 获取当前登陆用户
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static async Task<User> CurrentAsync(this IAbpSession abpSession)
        {

            var userRepository = IocManager.Instance.Resolve<IRepository<User, long>>();
            var u = await userRepository.FirstOrDefaultAsync(abpSession.GetUserId());
            if (u == null)
            {
                throw new UserFriendlyException("当前用户未登陆");
            }
            return u;
        }

    }
}
