using System.Collections.Generic;
using prod.Authorization.Users.Importing.Dto;
using prod.Dto;

namespace prod.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
