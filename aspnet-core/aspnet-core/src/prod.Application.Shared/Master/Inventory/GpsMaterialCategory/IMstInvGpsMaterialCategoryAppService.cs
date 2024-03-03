using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.GpsCategory.Dto;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.GpsMaterialCategory
{
    public interface IMstInvGpsMaterialCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<MstInvGpsMaterialCategoryDto>> GetAll(GetMstInvGpsMaterialCategoryInput input);
    }
}
