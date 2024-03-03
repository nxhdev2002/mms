using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Spp.Dto;

namespace prod.Master.Spp
{

    public interface IMstSppGlAccountAppService : IApplicationService
    {

        Task<PagedResultDto<MstSppGlAccountDto>> GetAll(GetMstSppGlAccountInput input);


    }

}


