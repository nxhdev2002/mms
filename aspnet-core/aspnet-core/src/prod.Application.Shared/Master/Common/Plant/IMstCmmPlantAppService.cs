using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{

    public interface IMstCmmPlantAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmPlantDto>> GetAll(GetMstCmmPlantInput input);

    }

}

