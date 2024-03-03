using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{

    public interface IMstInvCpsInventoryGroupAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvCpsInventoryGroupDto>> GetAll(GetMstInvCpsInventoryGroupInput input);


    }

}

