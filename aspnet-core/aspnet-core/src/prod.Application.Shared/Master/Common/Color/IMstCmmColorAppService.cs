using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm
{

    public interface IMstCmmColorAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmColorDto>> GetAll(GetMstCmmColorInput input);

        Task CreateOrEdit(CreateOrEditMstCmmColorDto input);

        Task Delete(EntityDto input);

    }

}

