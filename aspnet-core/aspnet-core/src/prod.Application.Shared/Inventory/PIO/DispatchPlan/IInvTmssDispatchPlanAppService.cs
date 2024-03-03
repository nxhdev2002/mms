using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.Tmss.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.Tmss
{
    public interface IInvTmssDispatchPlanAppService : IApplicationService
    {
        Task<PagedResultDto<InvTmssDispatchPlanDto>> GetAll(GetInvTmssDispatchPlanInput input);

    }

}


