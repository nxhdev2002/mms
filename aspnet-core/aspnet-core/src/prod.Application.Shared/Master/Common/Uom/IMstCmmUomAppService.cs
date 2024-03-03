using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{

    public interface IMstCmmUomAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmUomDto>> GetAll(GetMstCmmUomInput input);

        Task CreateOrEdit(CreateOrEditMstCmmUomDto input);

        Task Delete(EntityDto input);

    }

}


