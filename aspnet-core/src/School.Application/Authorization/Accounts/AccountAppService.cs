using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using MyCompanyName.AbpZeroTemplate.Authorization.Impersonation;
using School.Authorization.Accounts.Dto;
using School.Authorization.Users;
using School.MultiTenancy;

namespace School.Authorization.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountAppService : SchoolAppServiceBase, IAccountAppService
    {


        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRegistrationManager"></param>
        /// <param name="impersonationManager"></param>
        /// <param name="passwordHasher"></param>
        public AccountAppService(
            UserRegistrationManager userRegistrationManager,
            IImpersonationManager impersonationManager,
            IPasswordHasher<User> passwordHasher)
        {
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _passwordHasher = passwordHasher;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       
        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.UserName,
                input.Password
              
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null || user.PasswordResetCode.IsNullOrWhiteSpace() || user.PasswordResetCode != input.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            user.Password = _passwordHasher.HashPassword(user, input.Password);
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;

            await UserManager.UpdateAsync(user);

            return new ResetPasswordOutput
            {
                CanLogin = user.IsActive,
                UserName = user.UserName
            };
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
      
        [AbpAuthorize]
        public virtual async Task<ImpersonateOutput> Impersonate(ImpersonateInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TenantId)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNullAsync(AbpSession.ImpersonatorTenantId)
            };
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        private async Task<string> GetTenancyNameOrNullAsync(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenantAsync(tenantId.Value)).TenancyName : null;
        }

        private async Task<User> GetUserByChecking(string inputEmailAddress)
        {
            var user = await UserManager.FindByEmailAsync(inputEmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }

            return user;
        }
    }
}