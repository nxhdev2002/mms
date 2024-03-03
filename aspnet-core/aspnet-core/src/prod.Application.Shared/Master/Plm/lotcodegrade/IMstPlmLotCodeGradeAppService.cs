using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Plm.Dto;
using prod.Master.Plm.Dto;

namespace prod.Master.Plm
{

    public interface IMstPlmLotCodeGradeAppService : IApplicationService
    {

        Task<PagedResultDto<MstPlmLotCodeGradeDto>> GetAll(GetMstPlmLotCodeGradeInput input);

        Task CreateOrEdit(CreateOrEditMstPlmLotCodeGradeDto input);

        Task Delete(EntityDto input);

    }

}


