using System.Threading.Tasks;
using Abp.Application.Services;
using School.Authorization.Accounts.Dto;

namespace School.Authorization.Accounts
{/// <summary>
/// 
/// </summary>
    public interface IAccountAppService : IApplicationService
    {/// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

        Task<RegisterOutput> Register(RegisterInput input);
        /// <summary>
        /// 充值密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input);


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ImpersonateOutput> Impersonate(ImpersonateInput input);
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        Task<ImpersonateOutput> BackToImpersonator();

    }
}
