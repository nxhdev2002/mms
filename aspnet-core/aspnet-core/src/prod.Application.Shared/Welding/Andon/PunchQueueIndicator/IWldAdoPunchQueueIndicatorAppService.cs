using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Welding.Andon.PunchQueueIndicator.Dto;

namespace prod.Welding.Andon.WldPunchQueueIndicator
{
    public interface IWldAdoPunchQueueIndicatorAppService : IApplicationService
    {
        Task<PagedResultDto<WldAdoPunchQueueIndicatorDto>> getWldAdoPunchQueueIndicator();
        Task<List<WldAdoPunchQueueIndicatorV2Dto>> getWldAdoPunchQueueIndicatorV2();
    }
}
