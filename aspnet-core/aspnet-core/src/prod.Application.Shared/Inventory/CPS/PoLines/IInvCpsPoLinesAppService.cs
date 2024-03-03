using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CPS.Dto;

namespace prod.Inventory.CPS
{

    public interface IInvCpsPoLinesAppService : IApplicationService
    {

        Task<PagedResultDto<InvCpsPoLinesDto>> GetAll(GetInvCpsPoLinesInput input);

        Task CreateOrEdit(CreateOrEditInvCpsPoLinesDto input);

        Task Delete(EntityDto input);

    }

}


