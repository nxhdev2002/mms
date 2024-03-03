using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.GPS.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.GPS
{
    public interface IInvGpsStockRundownAppService : IApplicationService
    {
        Task<PagedResultDto<InvGpsStockRundownDto>> GetAll(GetInvGpsStockRundownInput input);
        Task CalculatorRundown();
    }

}


