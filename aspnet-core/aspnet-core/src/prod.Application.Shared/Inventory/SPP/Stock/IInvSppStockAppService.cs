using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.SPP.Stock.Dto;

namespace prod.Inventory.SPP.Stock
{
    public interface IInvSppStockAppService : IApplicationService
    {
        Task<PagedResultDto<InvSppStockDto>> GetAll(GetInvSppStockInput input);
    }
}
