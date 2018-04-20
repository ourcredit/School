using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Timing;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace School.Authorization.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123456";
        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        /// <summary>
        /// 是否超管
        /// </summary>
        public bool IsAdmin { get; set; } = false;
        /// <summary>
        /// 机构树节点权限
        /// </summary>
        public string TreeCode { get; set; }
        /// <summary>
        ///从属商铺id
        /// </summary>
        public int? ShopId { get; set; }
        /// <summary>
        /// hash 盐
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// 外链id
        /// </summary>
        public int? KeyId { get; set; }
        public string SignInToken { get; set; }
        #region 隐藏无用字段
        private new string AuthenticationSource { get; set; }
        private new string Surname { get; set; }
        private new bool IsEmailConfirmed { get; set; }
        private new string EmailConfirmationCode { get; set; }
        private new bool IsPhoneNumberConfirmed { get; set; }
        private new string NormalizedEmailAddress { get; set; }
        private new string NormalizedUserName { get; set; }
        #endregion

        //Can add application specific user properties here

        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }

        /// <summary>
        /// Creates admin <see cref="User"/> for a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                EmailAddress = emailAddress
            };

            user.SetNormalizedNames();

            return user;
        }
        /// <summary>
        /// create user for a tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static User CreateTenantUser(int tenantId, string userName,string name,string email)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = userName,
                Name = name,
                EmailAddress = email
            };

            user.SetNormalizedNames();

            return user;
        }
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public override void SetNewPasswordResetCode()
        {
            /* This reset code is intentionally kept short.
             * It should be short and easy to enter in a mobile application, where user can not click a link.
             */
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }
    }
}