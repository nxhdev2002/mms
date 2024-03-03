using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.DRM.StockRundown.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.DRM
{
    public interface IInvDrmStockRundownAppService : IApplicationService
    {
        Task<PagedResultDto<InvDrmStockRundownDto>> GetAll(GetInvDrmStockRundownInput input);

        Task<int> CalculatorRundown();
    }
}