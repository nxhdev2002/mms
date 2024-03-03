using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using prod.Inventory.SPP.Cost.Dto;

namespace prod.Inventory.SPP.Cost
{
    public interface IInvSppCostAppService : IApplicationService
    {
        Task<PagedResultDto<InvSppCostDto>> GetAll(GetInvSppCostInput input);
    }
}
