using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{

    public interface IMstCmmTransmissionTypeAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmTransmissionTypeDto>> GetAll(GetMstCmmTransmissionTypeInput input);

        //Task CreateOrEdit(CreateOrEditMstCmmTransmissionTypeDto input);

        //Task Delete(EntityDto input);

    }

}


