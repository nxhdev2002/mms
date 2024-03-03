using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Sps.Dto;

namespace prod.LogA.Sps
{

    public interface ILgaSpsStockAppService : IApplicationService
    {

        Task<PagedResultDto<LgaSpsStockDto>> GetAll(GetLgaSpsStockInput input);

        Task CreateOrEdit(CreateOrEditLgaSpsStockDto input);

        Task Delete(EntityDto input);

    }

}


