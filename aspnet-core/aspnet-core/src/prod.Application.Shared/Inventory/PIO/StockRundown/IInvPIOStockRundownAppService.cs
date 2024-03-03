using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.PIO.StockRundown.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockRundown
{
    public interface IInvPIOStockRundownAppService : IApplicationService
    {
        Task<PagedResultDto<InvPIOStockRundownDto>> GetAll(GetInvPIOStockRundownInput input);
    }
}
