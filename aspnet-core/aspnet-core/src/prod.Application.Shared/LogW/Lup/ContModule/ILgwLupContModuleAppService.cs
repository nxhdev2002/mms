using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Lup.Dto;

namespace prod.LogW.Lup.ContModule
{

    public interface ILgwLupContModuleAppService : IApplicationService
    {

        Task<PagedResultDto<LgwLupContModuleDto>> GetAll(GetLgwLupContModuleInput input);

        Task CreateOrEdit(CreateOrEditLgwLupContModuleDto input);

        Task Delete(EntityDto input);

    }

}


