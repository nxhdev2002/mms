using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.IHP.Dto;
using prod.Inventory.IHP.StockPart.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IHP.StockPart
{
    public interface IInvIhpStockPartAppService : IApplicationService
    {
        Task<PagedResultDto<InvIhpStockPartDto>> GetAll(GetInvIhpStockPartInput input);
    }
}
