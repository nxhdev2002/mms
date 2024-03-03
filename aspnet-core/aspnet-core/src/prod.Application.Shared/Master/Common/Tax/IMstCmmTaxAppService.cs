using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm
{

    public interface IMstCmmTaxAppService : IApplicationService
    {
        Task<PagedResultDto<MstCmmTaxDto>> GetAll(GetMstCmmTaxInput input);
    }

}
