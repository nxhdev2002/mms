using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdProductionPlanMonthlyAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdProductionPlanMonthlyDto>> GetAllProductionPlanMonthly(InvCkdProductionPlanMonthlyInput input);

        Task<List<InvCkdProductionPlanMonthlyImportDto>> ImportProductionPlanMonthlyFromExcel(byte[] fileBytes, string fileName);
    }
}
