﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm
{

    public interface IMstCmmBusinessParterAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmBusinessParterDto>> GetAll(GetMstCmmBusinessParterInput input);

        Task CreateOrEdit(CreateOrEditMstCmmBusinessParterDto input);

/*        Task Delete(EntityDto input);*/

    }

}