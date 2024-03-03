using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.PIO.Stock.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.Stock
{
    public interface IInvPIOStockAppService : IApplicationService
    {
        Task<PagedResultDto<InvPIOStockDto>> GetAll(GetInvPIOStockInput input);
    }
}


