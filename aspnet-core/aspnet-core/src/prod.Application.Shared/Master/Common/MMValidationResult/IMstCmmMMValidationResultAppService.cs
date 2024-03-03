using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.Dto;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    public interface IMstCmmMMValidationResultAppService : IApplicationService
    {
        Task<PagedResultDto<MstCommonMMValidationResultDto>> GetAll(GetMstCommonMMValidationResultInput input);
        
    }
}
