using Abp.Application.Services;
using Abp.Application.Services.Dto;

using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{
    public interface IMstCmmMaterialTypeAppService : IApplicationService
    {
        Task<PagedResultDto<MstCmmMaterialTypeDto>> GetAll(GetMstCmmMaterialTypeInput input);

    }

}

