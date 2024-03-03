using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using prod.Inventory.SPP.CostOfSaleSummary.Dto;

namespace prod.Inventory.SPP.CostOfSaleSummary
{
    public interface IInvSppCostOfSaleSummaryAppService : IApplicationService
    {
        Task<PagedResultDto<InvSppCostOfSaleSummaryDto>> GetAll(GetInvSppCostOfSaleSummaryInput input);
    }
}
