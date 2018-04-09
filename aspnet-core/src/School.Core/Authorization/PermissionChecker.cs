using Abp.Authorization;
using MyCompanyName.AbpZeroTemplate.Authorization.Users;
using School.Authorization.Roles;
using School.Authorization.Users;

namespace MyCompanyName.AbpZeroTemplate.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
