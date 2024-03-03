using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.IF.FQF3MM01.Dto;
using prod.Inventory.IF.FQF3MM05.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM01
{
    public interface IIF_FQF3MM01AppService : IApplicationService
    {
        Task<PagedResultDto<IF_FQF3MM01Dto>> GetAll(GetIF_FQF3MM01Input input);
        Task<PagedResultDto<GetIF_FQF3MM01_VALIDATE>> GetInvInterfaceFQF3MM01Validate(GetIF_FQF3MM01_VALIDATEInput input);

    }
}
