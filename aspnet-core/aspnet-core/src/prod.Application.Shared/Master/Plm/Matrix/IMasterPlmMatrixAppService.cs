using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Plm.Dto;

namespace prod.Master.Plm
{

    public interface IMasterPlmMatrixAppService : IApplicationService
    {

        Task<PagedResultDto<MasterPlmMatrixDto>> GetAll();

        Task CreateOrEdit(CreateOrEditMasterPlmMatrixDto input);

        Task Delete(EntityDto input);

    }

}


