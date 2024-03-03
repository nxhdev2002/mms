using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{

    public interface IMstInvCustomsStatusAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvCustomsStatusDto>> GetAll(GetMstInvCustomsStatusInput input);

    }

}


