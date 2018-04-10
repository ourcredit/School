using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto;
using School.Dto;

namespace School.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}