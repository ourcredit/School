using Abp.Authorization;
using School.Authorization.Roles;
using School.Authorization.Users;

namespace School.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
