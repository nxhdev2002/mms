using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Dto;

namespace prod.Master.Inventory
{

    public interface IMstInvPIOEmailAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvPIOEmailDto>> GetAll(GetMstInvPIOEmailInput input);

        Task CreateOrEdit(CreateOrEditMstInvPIOEmailDto input);

        Task Delete(EntityDto input);

    }

}


