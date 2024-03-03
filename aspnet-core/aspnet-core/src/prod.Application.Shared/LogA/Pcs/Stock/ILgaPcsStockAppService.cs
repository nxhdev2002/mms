using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Pcs.Dto;

namespace prod.LogA.Pcs.Stock
{

    public interface ILgaPcsStockAppService : IApplicationService
    {

        Task<PagedResultDto<LgaPcsStockDto>> GetAll(GetLgaPcsStockInput input);

        Task CreateOrEdit(CreateOrEditLgaPcsStockDto input);

        Task Delete(EntityDto input);

    }

}


