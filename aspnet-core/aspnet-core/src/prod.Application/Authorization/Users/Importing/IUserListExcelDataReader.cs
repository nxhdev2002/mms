using System.Collections.Generic;
using prod.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace prod.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
