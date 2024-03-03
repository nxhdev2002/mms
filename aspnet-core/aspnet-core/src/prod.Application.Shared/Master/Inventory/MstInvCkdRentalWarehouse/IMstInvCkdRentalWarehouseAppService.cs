using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    public interface IMstInvCkdRentalWarehouseAppService
    {
        Task<PagedResultDto<MstInvCkdRentalWarehouseDto>> GetAll(GetMstInvCkdRentalWarehouseInput input);
    }
}
