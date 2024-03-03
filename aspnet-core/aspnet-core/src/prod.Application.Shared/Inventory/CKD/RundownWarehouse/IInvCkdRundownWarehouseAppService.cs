using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.RundownWarehouse.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.RundownWarehouse
{
    public interface IInvCkdRundownWarehouseAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdRundownWarehouseDto>> GetAll(GetStockRundownWarehouseInput input);
    }
}
