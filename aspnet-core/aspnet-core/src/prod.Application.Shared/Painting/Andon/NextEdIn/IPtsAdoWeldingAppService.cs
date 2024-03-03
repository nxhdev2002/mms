using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Welding.Andon.NextEdIn.Dto;

namespace prod.Welding.Andon.NextEdIn
{
    public interface IPtsAdoWeldingAppService : IApplicationService
    {
        Task<PagedResultDto<GetNextEDInOutput>> GetNextEDIn();
    }
}
