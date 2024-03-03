using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.IF.FQF3MM04.Dto;
using prod.Inventory.IF.FQF3MM05.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM05
{
    public interface IIF_FQF3MM05AppService : IApplicationService
    {
        Task<PagedResultDto<IF_FQF3MM05Dto>> GetAll(GetIF_FQF3MM05Input input);

    }
}
