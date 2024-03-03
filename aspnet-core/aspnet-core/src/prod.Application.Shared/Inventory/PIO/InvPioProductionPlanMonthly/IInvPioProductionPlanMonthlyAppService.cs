using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.PIO.InvPioProductionPlanMonthly.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO
{
    public interface IInvPioProductionPlanMonthlyAppService : IApplicationService
    {
        Task<PagedResultDto<InvPioProductionPlanMonthlyDto>> GetAll(InvPioProductionPlanMonthlyInput input);

        Task<List<InvPioProductionPlanMonthlyImportDto>> ImportPioProductionPlanMonthlyFromExcel(byte[] fileBytes, string fileName);
    }
}
