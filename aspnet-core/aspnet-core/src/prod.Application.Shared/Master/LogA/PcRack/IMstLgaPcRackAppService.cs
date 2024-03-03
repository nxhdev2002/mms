using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{

    public interface IMstLgaPcRackAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgaPcRackDto>> GetAll(GetMstLgaPcRackInput input);

        Task CreateOrEdit(CreateOrEditMstLgaPcRackDto input);

        Task Delete(EntityDto input);

    }

}


