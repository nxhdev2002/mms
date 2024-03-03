using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.IF.FQF3MM07.Dto;

using prod.Inventory.IF.Dto;

namespace prod.Inventory.IF.FQF3MM07
{
    public interface IIF_FQF3MM07AppService : IApplicationService
    {
        Task<PagedResultDto<IF_FQF3MM07Dto>> GetAll(GetIF_FQF3MM07Input input);

    }

}


