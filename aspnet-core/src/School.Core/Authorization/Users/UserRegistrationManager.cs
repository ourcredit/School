using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Authorization.Roles;
using School.Configuration;
using School.MultiTenancy;

namespace School.Authorization.Users
{
    public class UserRegistrationManager : SchoolDomainServiceBase
    {
        public IAbpSession AbpSession { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRegistrationManager(
            TenantManager tenantManager, 
            UserManager userManager,
            RoleManager roleManager, 
            IPasswordHasher<User> passwordHasher)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;

            AbpSession = NullAbpSession.Instance;
        }
        public async Task<User> RegisterAdminAsync(string name,
            string userName, string plainPassword,string salt,string treeCode,int keyId)
        {
            CheckForTenant();
            // CheckSelfRegistrationIsEnabled();

            var tenant = await GetActiveTenantAsync();
            // var isNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault);

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                EmailAddress = $"{userName}.{name}@qq.com",
                IsActive = true,
                Salt = salt,
                TreeCode = treeCode,KeyId=1,
                IsAdmin = true,
                UserName = userName,
                IsEmailConfirmed = false,
                Password = plainPassword,
                Roles = new List<UserRole>()
            };

          //  user.SetNormalizedNames();
             user.Roles.Add(new UserRole(tenant.Id, user.Id, 2));
            CheckErrors(await _userManager.CreateAsync(user));
          //  await CurrentUnitOfWork.SaveChangesAsync();
            //Notifications
          //  await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());

            return user;
        }

        public async Task<User> RegisterAsync(string name,
            string userName, string plainPassword )
        {
            CheckForTenant();
           // CheckSelfRegistrationIsEnabled();

            var tenant = await GetActiveTenantAsync();
           // var isNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault);

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                EmailAddress = $"{userName}.{name}@qq.com",
                IsActive = true,
                UserName = userName,
                IsEmailConfirmed = false,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            user.Password = _passwordHasher.HashPassword(user, plainPassword);

            foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            CheckErrors(await _userManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync();

           

            //Notifications

            return user;
        }

        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration))
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

      

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
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

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
