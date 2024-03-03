using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CPS.Dto;

namespace prod.Inventory.CPS
{

    public interface IInvCpsInventoryItemPriceAppService : IApplicationService
    {
        Task<PagedResultDto<InvCpsInventoryItemPriceDto>> GetCpsInventoryItemPriceSearch(GetInvCpsInventoryItemPriceInput input);
    }

}


