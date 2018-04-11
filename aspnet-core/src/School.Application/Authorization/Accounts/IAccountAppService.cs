using System.Threading.Tasks;
using Abp.Application.Services;
using School.Authorization.Accounts.Dto;

namespace School.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {

        Task<RegisterOutput> Register(RegisterInput input);


        Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input);



        Task<ImpersonateOutput> Impersonate(ImpersonateInput input);

        Task<ImpersonateOutput> BackToImpersonator();

    }
}
