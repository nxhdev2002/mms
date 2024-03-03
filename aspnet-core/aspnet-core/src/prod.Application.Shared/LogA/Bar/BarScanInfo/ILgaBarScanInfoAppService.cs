using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Bar.Dto;

namespace prod.LogA.Bar
{

    public interface ILgaBarScanInfoAppService : IApplicationService
    {

        Task<PagedResultDto<LgaBarScanInfoDto>> GetAll(GetLgaBarScanInfoInput input);

        Task CreateOrEdit(CreateOrEditLgaBarScanInfoDto input);

        Task Delete(EntityDto input);

    }

}