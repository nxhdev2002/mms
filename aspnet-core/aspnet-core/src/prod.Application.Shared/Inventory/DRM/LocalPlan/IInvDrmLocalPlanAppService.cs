using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.DRM.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.DRM
{
    public interface IInvDrmLocalPlanAppService : IApplicationService
    {
        Task<PagedResultDto<InvDrmLocalPlanDto>> GetAll(GetInvDrmLocalPlanInput input);
    }
}