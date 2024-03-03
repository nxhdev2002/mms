using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Master.Inventory.Dto;

namespace prod.Master.Inventory
{

    public interface IMstInvCpsInventoryItemsAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvCpsInventoryItemsDto>> GetAll(GetMstInvCpsInventoryItemsInput input);


    }

}