using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Plm.Dto;
using prod.Master.Plm.Dto;

namespace prod.Master.Plm
{

    public interface IMasterPlmPartAppService : IApplicationService
    {

        Task<PagedResultDto<MasterPlmPartDto>> GetAll(GetMasterPlmPartInput input);

        Task CreateOrEdit(CreateOrEditMasterPlmPartDto input);

        Task Delete(EntityDto input);

    }

}