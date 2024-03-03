using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.LogA.Bp2.Dto;
using prod.LogA.Bp2.PxPUpPlan.Dto;

namespace prod.LogA.Bp2.PxPUpPlan
{
    public interface ILgaBp2PxPUpPlanAppService : IApplicationService
    {
        Task<PagedResultDto<LgaBp2PxPUpPlanDto>> GetAll(GetLgaBp2PxPUpPlanInput input);
    }
}
