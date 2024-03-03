using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.PIO.StockRundownTransaction.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockRundownTransaction
{
    public interface IInvPIOStockRundownTransactionAppService : IApplicationService
    {

        Task<PagedResultDto<InvPIOStockRundownTransactionDto>> GetAll(GetInvPIOStockRundownTransactionInput input);

    }
}
