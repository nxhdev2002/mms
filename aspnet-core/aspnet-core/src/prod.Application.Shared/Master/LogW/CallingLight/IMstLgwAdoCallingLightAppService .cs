using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{
    public interface IMstLgwAdoCallingLightAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgwAdoCallingLightDto>> GetAll(GetMstLgwAdoCallingLightInput input);

        Task CreateOrEdit(CreateOrEditMstLgwAdoCallingLightDto input);

        Task Delete(EntityDto input);

    }
}
