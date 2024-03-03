using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv
{

    public interface IMstInvGpsCalendarAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvGpsCalendarDto>> GetAll(GetMstInvGpsCalendarInput input);

        Task CreateOrEdit(CreateOrEditMstInvGpsCalendarDto input);

        Task Delete(EntityDto input);

    }

}


