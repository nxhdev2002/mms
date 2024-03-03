using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Dto;

namespace prod.Master.Inventory
{

    public interface IMstInvHrPositionAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvHrPositionDto>> GetAll(GetMstInvHrPositionInput input);

/*        Task CreateOrEdit(CreateOrEditMstInvHrPositionDto input);*/

        /*Task Delete(EntityDto input);*/

    }

}


