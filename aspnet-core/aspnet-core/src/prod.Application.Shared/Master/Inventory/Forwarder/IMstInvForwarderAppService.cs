using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.Forwarder.Dto;
using System.Threading.Tasks;

namespace prod.Master.Inventory.Forwarder
{

    public interface IMstInvForwarderAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvForwarderDto>> GetAll(GetMstInvForwarderInput input);

    }

}


