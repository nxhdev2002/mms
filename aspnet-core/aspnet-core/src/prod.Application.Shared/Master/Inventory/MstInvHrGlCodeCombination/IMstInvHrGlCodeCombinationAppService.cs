using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inventory.Dto;

namespace prod.Master.Inventory
{

    public interface IMstInvHrGlCodeCombinationAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvHrGlCodeCombinationDto>> GetAll(GetMstInvHrGlCodeCombinationInput input);

    }

}


