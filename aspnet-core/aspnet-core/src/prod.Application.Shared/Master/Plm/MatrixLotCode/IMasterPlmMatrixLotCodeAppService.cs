using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Plm.Dto;

namespace prod.Master.Plm
{

    public interface IMasterPlmMatrixLotCodeAppService : IApplicationService
    {

        Task<PagedResultDto<MasterPlmMatrixLotCodeDto>> GetAll();

        Task CreateOrEdit(CreateOrEditMasterPlmMatrixLotCodeDto input);

        Task Delete(EntityDto input);

    }

}


