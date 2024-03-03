using Abp.Application.Services.Dto;
using Abp.Application.Services;
using prod.Inventory.IF.FQF3MM03.Dto;
using prod.Inventory.IF.FQF3MM04.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.IF.FQF3MM07.Dto;

namespace prod.Inventory.IF.FQF3MM03
{
    public interface IIF_FQF3MM03AppService : IApplicationService
    {
        Task<PagedResultDto<IF_FQF3MM03Dto>> GetAll(GetIF_FQF3MM03Input input);

        Task<PagedResultDto<BusinessDataDto>> GetViewBusinessData(BusinessDataInput input);

    }
}
