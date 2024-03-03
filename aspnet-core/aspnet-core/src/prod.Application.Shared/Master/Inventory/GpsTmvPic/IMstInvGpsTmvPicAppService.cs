using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv
{

    public interface IMstInvGpsTmvPicAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvGpsTmvPicDto>> GetAll(GetMstInvGpsTmvPicInput input);

        Task CreateOrEdit(CreateOrEditMstInvGpsTmvPicDto input);

        Task Delete(EntityDto input);

    }

}

