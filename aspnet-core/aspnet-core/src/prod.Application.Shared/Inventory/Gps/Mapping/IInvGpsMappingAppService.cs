using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.Gps.Mapping.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.Mapping
{
    public interface IInvGpsMappingAppService : IApplicationService
    {
        Task<PagedResultDto<InvGpsMappingDto>> GetAll(GetInvGpsMappingInput input);
    }
}
