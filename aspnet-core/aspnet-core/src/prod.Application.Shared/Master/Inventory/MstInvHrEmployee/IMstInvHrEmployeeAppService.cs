using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inventory.MstInvHrEmployee.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.MstInvHrEmployee
{
    public interface IMstInvHrEmployeeAppService : IApplicationService
    {
        Task<PagedResultDto<MstInvHrEmployeeDto>> GetAll(MstInvHrEmployeeInput input);
    }
}
