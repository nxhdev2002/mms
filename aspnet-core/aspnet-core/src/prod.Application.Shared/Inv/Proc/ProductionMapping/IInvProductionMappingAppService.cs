using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;

using prod.Inv.Proc.Dto;
using System;
using prod.Inv.D125.Dto;

namespace prod.Inv.Proc
{
    public interface IInvProductionMappingAppService
    {
        Task<PagedResultDto<InvProductionMappingDto>> GetAll(InvProductionMappingInput input);

        Task<PagedResultDto<InvPeriodDto>> GetIdInvPeriod();


    }
}
