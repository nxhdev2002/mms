using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.HrTitles.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.HrTitles
{
    public interface IMstInvHrTitlesAppService : IApplicationService
    {
        Task<PagedResultDto<MstInvHrTitlesDto>> GetAll(MstInvHrTitlesInput input);

    }
}
