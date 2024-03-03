using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using System.Threading.Tasks;

using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm
{

    public interface IMstCmmEngineMasterAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmEngineMasterDto>> GetAll(GetMstCmmEngineMasterInput input);

        Task CreateOrEdit(CreateOrEditMstCmmEngineMasterDto input);

        Task Delete(EntityDto input);

    }

}