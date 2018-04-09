using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace School.Controllers
{
    public abstract class SchoolControllerBase: AbpController
    {
        protected SchoolControllerBase()
        {
            LocalizationSourceName = SchoolConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
