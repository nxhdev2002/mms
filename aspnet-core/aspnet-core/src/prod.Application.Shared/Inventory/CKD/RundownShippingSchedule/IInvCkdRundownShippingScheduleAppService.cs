using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.RundownShippingSchedule.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.RundownShippingSchedule
{
    public interface IInvCkdRundownShippingScheduleAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdRundownShippingScheduleDto>> GetAll(GetStockRundownShippingScheduleInput input);
    }
}
