using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.LogW.Lup.Dto;

namespace prod.LogW.Lup
{
    public interface ILotMakingTabletAppService : IApplicationService
    {
        Task<PagedResultDto<LotMakingTabletDto>> GetMkDataLotUpPlan();
        Task<PagedResultDto<LotMakingTabletDto>> GetMkModuleListDataLotUpPlan(string prod_line, string lot_no, int lot_id);
        Task FinishLotMk(int lot_id);
    }
}
