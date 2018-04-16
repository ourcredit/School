using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using School.Authorization.Roles;
using School.Authorization.Users;
using School.MultiTenancy;

namespace School.Authorization
{
    /// <summary>
    /// 登陆管理
    /// </summary>
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="multiTenancyConfig"></param>
        /// <param name="tenantRepository"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="settingManager"></param>
        /// <param name="userLoginAttemptRepository"></param>
        /// <param name="userManagementConfig"></param>
        /// <param name="iocResolver"></param>
        /// <param name="roleManager"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="claimsPrincipalFactory"></param>
        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,
            UserClaimsPrincipalFactory claimsPrincipalFactory)
            : base(
                userManager,
                multiTenancyConfig,
                tenantRepository,
                unitOfWorkManager,
                settingManager,
                userLoginAttemptRepository,
                userManagementConfig,
                iocResolver,
                passwordHasher,
                roleManager,
                claimsPrincipalFactory)
        {

        }

        /// <summary>
        /// 重写登陆方法
        /// </summary>
        /// <param name="userNameOrEmailAddress"></param>
        /// <param name="plainPassword"></param>
        /// <param name="tenancyName"></param>
        /// <param name="shouldLockout"></param>
        /// <returns></returns>
        public override async Task<AbpLoginResult<Tenant, User>> LoginAsync(string userNameOrEmailAddress,
            string plainPassword, string tenancyName = null,
            bool shouldLockout = true)
        {
            var result = await LoginAsyncOverride(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttempt(result, tenancyName, userNameOrEmailAddress);
            return result;
        }

        /// <summary>
        /// 重写登陆验证
        /// </summary>
        /// <param name="userNameOrEmailAddress"></param>
        /// <param name="plainPassword"></param>
        /// <param name="tenancyName"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        protected virtual async Task<AbpLoginResult<Tenant, User>> LoginAsyncOverride(string userNameOrEmailAddress,
            string plainPassword, string tenancyName, bool isAdmin=false)
        {
            if (userNameOrEmailAddress.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            //Get and check tenant
            Tenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?) null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await UserManager.InitializeOptionsAsync(tenantId);
                var user = await UserManager.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                
                if (user == null)
                {
                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                }
                if (isAdmin != user.IsAdmin)
                {
                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.UserPhoneNumberIsNotConfirmed, tenant);
                }
                if (await UserManager.IsLockedOutAsync(user))
                {
                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }
    }
}