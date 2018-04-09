using System.Security.Claims;
using MyCompanyName.AbpZeroTemplate.Authorization.Users;
using School.Authorization.Users;

namespace MyCompanyName.AbpZeroTemplate.Authorization.Impersonation
{
    public class UserAndIdentity
    {
        public User User { get; set; }

        public ClaimsIdentity Identity { get; set; }

        public UserAndIdentity(User user, ClaimsIdentity identity)
        {
            User = user;
            Identity = identity;
        }
    }
}