using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{

    public interface IMstLgwEciPartModuleAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgwEciPartModuleDto>> GetAll(GetMstLgwEciPartModuleInput input);

        Task<FileDto> GetEciPartModuleToExcel(MstLgwEciPartModuleExportInput input);


    }

}


