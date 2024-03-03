using System.Collections.Generic;
using prod.Authorization.Users.Dto;
using prod.Dto;

namespace prod.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}