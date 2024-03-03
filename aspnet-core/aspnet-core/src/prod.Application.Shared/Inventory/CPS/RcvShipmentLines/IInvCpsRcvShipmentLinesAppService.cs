using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CPS.Dto;

namespace prod.Inventory.CPS
{

    public interface IInvCpsRcvShipmentLinesAppService : IApplicationService
    {

        Task<PagedResultDto<InvCpsRcvShipmentLinesDto>> GetAll(GetInvCpsRcvShipmentLinesInput input);

        Task CreateOrEdit(CreateOrEditInvCpsRcvShipmentLinesDto input);

        Task Delete(EntityDto input);

    }

}


