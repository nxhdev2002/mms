using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Common.Dto;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{

    public interface IMstCmmValuationTypeAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmValuationTypeDto>> GetAll(GetMstCmmValuationTypeInput input);
    }

}

