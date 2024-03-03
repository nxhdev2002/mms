using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{

    public interface IMstLgaBarUserAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgaBarUserDto>> GetAll(GetMstLgaBarUserInput input);

        Task CreateOrEdit(CreateOrEditMstLgaBarUserDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetBarUserToExcel(GetMstLgaBarUserExcelInput input);
    }

}