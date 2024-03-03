using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.ReceivingPhysicalStock.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdReceivingPhysicalStockAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdReceivingPhysicalStockDto>> GetDataReceivingPhysicalStock(InvCkdReceivingPhysicalStockInputDto input);
    }
}
