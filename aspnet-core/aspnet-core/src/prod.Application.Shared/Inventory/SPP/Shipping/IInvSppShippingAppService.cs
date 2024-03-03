using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.SPP.Shipping.Dto;

namespace prod.Inventory.SPP.Shipping
{
    public interface IInvSppShippingAppService : IApplicationService
    {
        Task<PagedResultDto<InvSppShippingDto>> GetAll(GetInvSppShippingInput input);
    }
}
