using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{

    public interface IMstCmmFuelTypeAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmFuelTypeDto>> GetAll(GetMstCmmFuelTypeInput input);

        //Task CreateOrEdit(CreateOrEditMstCmmFuelTypeDto input);

        //Task Delete(EntityDto input);

    }

}

