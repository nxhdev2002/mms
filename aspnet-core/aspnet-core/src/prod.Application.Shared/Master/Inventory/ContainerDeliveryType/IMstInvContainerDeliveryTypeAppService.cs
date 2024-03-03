using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.ContainerDeliveryType.Dto;
using System.Threading.Tasks;

namespace prod.Master.Inventory.ContainerDeliveryType
{

    public interface IMstInvContainerDeliveryTypeAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvContainerDeliveryTypeDto>> GetAll(GetMstInvContainerDeliveryTypeInput input);

    }

}


