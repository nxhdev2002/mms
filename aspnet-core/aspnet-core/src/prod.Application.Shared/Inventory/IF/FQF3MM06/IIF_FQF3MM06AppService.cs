using Abp.Application.Services.Dto;
using Abp.Application.Services;
using prod.Inventory.IF.FQF3MM04.Dto;
using prod.Inventory.IF.FQF3MM06.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM06
{
    public interface IIF_FQF3MM06AppService : IApplicationService
    {
        Task<PagedResultDto<IF_FQF3MM06Dto>> GetAll(GetIF_FQF3MM06Input input);

    }
}
