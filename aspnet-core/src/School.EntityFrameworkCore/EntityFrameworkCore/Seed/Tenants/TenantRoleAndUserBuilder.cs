﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using School.Authorization;
using School.Authorization.Roles;
using School.Authorization.Users;
using School.Models;

namespace School.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly SchoolDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(SchoolDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers() ;
        }

        private  void CreateRolesAndUsers()
        {
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new AppAuthorizationProvider(true))
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user
            CreateSuperAdminsFromDb(adminRole);
            //var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            //if (adminUser == null)
            //{
            //    adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
            //    adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
            //    adminUser.IsEmailConfirmed = true;
            //    adminUser.IsActive = true;
            //    _context.Users.Add(adminUser);
            //    _context.SaveChanges();

            //    // Assign Admin role to admin user
            //    _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
            //    _context.SaveChanges();

            //    // User account of admin user
            //    if (_tenantId == 1)
            //    {
            //        _context.UserAccounts.Add(new UserAccount
            //        {
            //            TenantId = _tenantId,
            //            UserId = adminUser.Id,
            //            UserName = AbpUserBase.AdminUserName,
            //            EmailAddress = adminUser.EmailAddress
            //        });
            //        _context.SaveChanges();
            //    }
            //}
        }

        private  void CreateSuperAdminsFromDb(Role adminRole)
        {
            var sql = @"SELECT
	a.shop_name,
	b.user_id,
	b.email,
	b.user_name,
	b.`password`,ec_salt
	
FROM
	dsc_drp_shop a
	LEFT JOIN dsc_users b ON a.user_id = b.user_id";
            var result =  DapperHelper.GetSqlResult<dsc_drp_shop>(sql);
            foreach (var item in result.Items)
            {
                var temp =  _context.Users.IgnoreQueryFilters().FirstOrDefault(u =>
                u.TenantId == _tenantId && u.UserName == item.user_name);
                var family =  _context.OperatorTrees.IgnoreQueryFilters().FirstOrDefault(c => c.TreeName == item.shop_name);
                if (family == null)
                {
                    family = new OperatorTree()
                    {
                        TreeLength = 1,
                        TreeName = item.shop_name,
                        TreeCode = Guid.NewGuid().ToString("D").Split('-').Last()
                    };
                     _context.OperatorTrees.Add(family);
                    _context.SaveChanges();
                }
                if (temp != null) continue;

                temp = User.CreateTenantUser(_tenantId, item.user_name, item.user_name, item.email);
                temp.Password =item.password;
                temp.IsEmailConfirmed = true;
                temp.IsActive = true;
                temp.IsAdmin = true;
                temp.TreeCode = family.TreeCode;
                temp.Salt = item.ec_salt;
                temp.KeyId = item.user_id;
                _context.Users.Add(temp);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, temp.Id, adminRole.Id));
                _context.SaveChanges();
                // User account of admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = temp.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = temp.EmailAddress
                    });
                    _context.SaveChanges();
                }
            }
        }
    }
    /// <summary>
    /// temp modal
    /// </summary>
    public class dsc_drp_shop
    {
        public string shop_name { get; set; }
        public int user_id { get; set; }
        public string email { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string ec_salt { get; set; }
    }
}
