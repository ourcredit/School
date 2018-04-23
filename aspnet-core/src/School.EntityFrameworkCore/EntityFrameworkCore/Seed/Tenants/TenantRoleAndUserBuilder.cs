using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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
          
            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;
                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                // User account of admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }
            }

            //  CreateSuperAdminsFromDb(adminRole);
        }

        private  void CreateSuperAdminsFromDb(Role adminRole)
        {
            var sql = @"SELECT
	a.user_id,
	a.email,
	a.user_name,
	a.`password`,
	a.ec_salt,
	b.shop_name ,b.ru_id
FROM
	dsc_admin_user a
	LEFT JOIN dsc_seller_shopinfo b ON a.ru_id = b.ru_id";
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
                        ShopId = item.ru_id,
                        TreeName = item.shop_name,
                        TreeCode = Guid.NewGuid().ToString("D").Split('-').Last()
                    };
                     _context.OperatorTrees.Add(family);
                    _context.SaveChanges();
                }
                if (temp != null) continue;

                temp = User.CreateTenantUser(_tenantId, item.user_name, item.user_name, item.email);
                temp.Password =item.password;
                temp.ShopId = item.ru_id;
                temp.IsEmailConfirmed = true;
                temp.IsActive = true;
                temp.IsAdmin = true;
                temp.TreeCode = family.TreeCode;
                temp.EmailAddress = item.user_name + "@" + item.user_id + ".com";
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
  
}
