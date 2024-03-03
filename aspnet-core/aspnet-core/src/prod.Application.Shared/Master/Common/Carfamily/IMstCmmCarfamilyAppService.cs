using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Common.Dto;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{

    public interface IMstCmmCarfamilyAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmCarfamilyDto>> GetAll(GetMstCmmCarfamilyInput input);

/*        Task CreateOrEdit(CreateOrEditMstCmmCarfamilyDto input);

        Task Delete(EntityDto input);
*/
    }

}


