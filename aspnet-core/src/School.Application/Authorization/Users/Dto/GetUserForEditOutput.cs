using System;
using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto;

namespace School.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        public Guid? ProfilePictureId { get; set; }

        public UserEditDto User { get; set; }

        public UserRoleDto[] Roles { get; set; }


        public List<string> MemberedOrganizationUnits { get; set; }
    }
}