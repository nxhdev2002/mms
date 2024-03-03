using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.PIO.StockTransaction.Dto;

namespace prod.Inventory.PIO.StockIssuing
{

    public interface IInvPIOStockIssuingAppService : IApplicationService
    {

        Task<PagedResultDto<InvPIOStockTransactionDto>> GetAll(GetInvPIOStockTransactionInput input);


    }

}


