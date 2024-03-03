using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.Dto;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    public interface IMstCmmDevanningCaseTypeAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmDevanningCaseTypeDto>> GetAll(GetMstCmmDevanningCaseTypeInput input);

        Task CreateOrEdit(CreateOrEditMstCmmDevanningCaseTypeDto input);

        Task Delete(EntityDto input);

    }

}


