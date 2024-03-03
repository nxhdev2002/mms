using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.ContainerStatus.Dto;
using System.Threading.Tasks;

namespace prod.Master.Inventory.ContainerStatus
{

    public interface IMstInvContainerStatusAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvContainerStatusDto>> GetAll(GetMstInvContainerStatusInput input);

    }

}


