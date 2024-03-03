using Abp.Application.Services.Dto;
using Abp.Application.Services;
using prod.Inventory.CKD.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Inv.D125.Dto;

namespace prod.Inventory.CKD
{
    public interface IInvCkdStockBalanceAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdStockBalanceDto>> GetDataBalance(InvCkdStockBalanceInputDto input);

        //Task<PagedResultDto<InvPeriodDto>> GetIdInvPeriod();
    }

}
