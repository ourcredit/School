using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Services;

namespace School
{
    public abstract class SchoolDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected SchoolDomainServiceBase()
        {
            LocalizationSourceName = SchoolConsts.LocalizationSourceName;
        }
    }
   
}
