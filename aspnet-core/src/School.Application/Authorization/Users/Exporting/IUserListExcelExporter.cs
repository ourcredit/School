using System.Collections.Generic;
using School.Authorization.Users.Dto;
using School.Dto;

namespace School.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}