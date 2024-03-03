using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv
{

    public interface IMstInvGpsScreenSettingAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvGpsScreenSettingDto>> GetAll(GetMstInvGpsScreenSettingInput input);

        Task CreateOrEdit(CreateOrEditMstInvGpsScreenSettingDto input);

        Task Delete(EntityDto input);

    }

}


