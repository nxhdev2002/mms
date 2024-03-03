using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdContainerDeliveryGateInAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdContainerDeliveryGateInDto>> GetAll(GetInvCkdContainerDeliveryGateInInput input);



    }

}


