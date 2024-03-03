using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.Gps.StockReceivingTransDetails.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.StockReceivingTransDetails
{
    public interface IInvGpsStockReceivingTransactionAppService : IApplicationService
    {
        Task<PagedResultDto<InvGpsStockReceivingTransactionDto>> GetAll(GetStockReceivingTransactionInput input);

        Task<List<InvGpsStockReceivingTransactionDto>> ImportInvGpsStockReceivingTransFromExcel(byte[] fileBytes, string fileName);
    }
}
