using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.PIO.StockReceiving.Dto;
using prod.Inventory.PIO.StockTransaction.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockTransaction
{
    public interface IInvPIOStockTransactionAppService : IApplicationService
    {

        Task<PagedResultDto<InvPIOStockTransactionDto>> GetAll(GetInvPIOStockTransactionInput input);

    }
}
