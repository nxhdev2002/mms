using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.RundownShippingResult.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.RundownShippingResult
{
    public interface IInvCkdRundownShippingResultAppService  : IApplicationService
    {
        Task<PagedResultDto<InvCkdRundownShippingResultDto>> GetAll(GetStockRundownShippingResultInput input);
    }
}
