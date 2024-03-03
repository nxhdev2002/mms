using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.LotPart.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.LotPart
{
    public interface IMstInvLotPartAppService : IApplicationService
    {
        Task<PagedResultDto<MstInvLotPartDto>> GetAll(GetMstInvLotPartInput input);
    }
}
