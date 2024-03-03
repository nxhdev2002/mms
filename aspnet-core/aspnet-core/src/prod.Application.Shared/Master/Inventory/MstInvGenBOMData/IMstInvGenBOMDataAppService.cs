using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.MstInvGenBOMData.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.MstInvGenBOMData
{
    public  interface IMstInvGenBOMDataAppService : IApplicationService
    {
        Task<PagedResultDto<MstInvGenBOMDataDto>> GetAll(GetInvGenBOMDataInput input);
    }
}
