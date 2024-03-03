using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Master.Common.MaterialMaster
{

    public interface IMstCmmMaterialMasterAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmMaterialMasterDto>> GetAll(GetMstCmmMaterialMasterInput input);

        Task<PagedResultDto<MstCmmMaterialMasterDto>> GetDataMaterialbyId(long? IdMaterial);

    }

}

