using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.GPS.Dto;

namespace prod.Inventory.GPS
{

    public interface IInvGpsKanbanAppService : IApplicationService
    {

        Task<PagedResultDto<InvGpsKanbanDto>> GetAll(GetInvGpsKanbanInput input);

        Task CreateOrEdit(CreateOrEditInvGpsKanbanDto input);

        Task Delete(EntityDto input);

    }

}


