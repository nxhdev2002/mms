using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.GpsCategory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.GpsCategory
{
    public interface IMstInvGpsCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<MstInvGpsCategoryDto>> GetAll(GetMstInvGpsCategoryInput input);
    }
}
