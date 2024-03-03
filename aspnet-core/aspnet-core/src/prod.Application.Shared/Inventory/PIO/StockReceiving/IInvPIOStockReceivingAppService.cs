using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.PIO.StockReceiving.Dto;
using prod.Inventory.PIO.StockTransaction.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockReceiving
{
    public interface  IInvPIOStockReceivingAppService : IApplicationService
    {
        Task<PagedResultDto<InvPIOStockReceivingDto>> GetAll(GetInvPIOStockReceivingInput input);
    }
}
